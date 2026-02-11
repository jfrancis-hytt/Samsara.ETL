using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;
using Samsara.Infrastructure.Samsara.Requests;

namespace Samsara.Infrastructure.Services;

public class SensorHistoryService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly ISensorHistoryReadingRepository _sensorHistoryReadingRepository;

    public SensorHistoryService(
        ISamsaraClient samsaraClient,
        ISensorHistoryReadingRepository sensorHistoryReadingRepository)
    {
        _samsaraClient = samsaraClient;
        _sensorHistoryReadingRepository = sensorHistoryReadingRepository;
    }

    private const int MaxConcurrency = 5;

    public async Task<List<SensorHistoryDto>> SyncSensorHistoryAsync(
        List<long> sensorIds,
        long startMs,
        long endMs,
        long stepMs,
        CancellationToken ct = default)
    {
        // Fetch sensors in parallel
        using var semaphore = new SemaphoreSlim(MaxConcurrency);
        var tasks = sensorIds.Select(async sensorId =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                return await FetchSensorHistoryAsync(sensorId, startMs, endMs, stepMs, ct);
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
        long stepMs,
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
            StepMs: stepMs,
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
