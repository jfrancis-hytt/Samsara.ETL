using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;
using Samsara.Infrastructure.Samsara.Options;
using Samsara.Infrastructure.Samsara.Requests;

namespace Samsara.Infrastructure.Services;

public class SensorHistoryService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ISensorRepository _sensorRepository;
    private readonly ISensorHistoryReadingRepository _sensorHistoryReadingRepository;
    private readonly SensorHistoryOptions _options;
    private readonly ILogger<SensorHistoryService> _logger;

    public SensorHistoryService(
        ISamsaraClient samsaraClient,
        ISensorRepository sensorRepository,
        ISensorHistoryReadingRepository sensorHistoryReadingRepository,
        IOptions<SensorHistoryOptions> options,
        ILogger<SensorHistoryService> logger)
    {
        _samsaraClient = samsaraClient;
        _sensorRepository = sensorRepository;
        _sensorHistoryReadingRepository = sensorHistoryReadingRepository;
        _options = options.Value;
        _logger = logger;
    }

    private const int MaxConcurrency = 5;

    public async Task<List<SensorHistoryDto>> SyncSensorHistoryAsync(CancellationToken ct = default)
    {
        var sensors = await _sensorRepository.GetAllAsync();
        var sensorIds = sensors.Select(s => s.SensorId).ToList();

        if (sensorIds.Count == 0)
        {
            _logger.LogWarning("No sensors found in database — skipping history sync");
            return [];
        }

        var now = DateTimeOffset.UtcNow;
        var endMs = now.ToUnixTimeMilliseconds();
        var startMs = now.AddHours(-_options.LookbackHours).ToUnixTimeMilliseconds();

        // Fetch sensors in parallel
        using var semaphore = new SemaphoreSlim(MaxConcurrency);
        var tasks = sensorIds.Select(async sensorId =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                return await FetchSensorHistoryAsync(sensorId, startMs, endMs, ct);
            }
            finally
            {
                semaphore.Release();
            }
        });
        var results = await Task.WhenAll(tasks);
        var dtos = results.SelectMany(r => r).ToList();

        // Batch insert all readings at once
        var entities = dtos.Select(dto => new SensorHistoryReadingEntity
        {
            SensorId = dto.SensorId,
            TimeMs = dto.TimeMs,
            ProbeTemperature = dto.ProbeTemperature,
            AmbientTemperature = dto.AmbientTemperature
        });

        await _sensorHistoryReadingRepository.InsertBatchAsync(entities);

        return dtos;
    }

    private async Task<List<SensorHistoryDto>> FetchSensorHistoryAsync(
        long sensorId,
        long startMs,
        long endMs,
        CancellationToken ct)
    {
        var series = new List<SensorHistorySeries>
        {
            new("probeTemperature", sensorId.ToString()),
            new("ambientTemperature", sensorId.ToString())
        };

        var request = new SensorHistoryRequest(
            FillMissing: "withNull",
            Series: series,
            StepMs: _options.StepMs,
            StartMs: startMs,
            EndMs: endMs
        );

        var response = await _samsaraClient.GetSensorHistoryAsync(request, ct);

        var dtos = new List<SensorHistoryDto>();

        foreach (var result in response.Results)
        {
            if (result.TimeMs is null) continue;

            var dto = new SensorHistoryDto(
                SensorId: sensorId,
                TimeMs: result.TimeMs.Value,
                ProbeTemperature: result.Series.Count > 0 ? result.Series[0] : null,
                AmbientTemperature: result.Series.Count > 1 ? result.Series[1] : null
            );

            if (dto.ProbeTemperature is null && dto.AmbientTemperature is null)
                continue;

            dtos.Add(dto);
        }

        return dtos;
    }
}
