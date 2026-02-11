using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Samsara.Options;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.SensorHistory;

public class SensorHistoryJob
{
    private readonly SensorHistoryService _service;
    private readonly ILogger<SensorHistoryJob> _logger;
    private readonly ISensorRepository _sensorRepository;
    private readonly SensorHistoryOptions _options;

    public SensorHistoryJob(
        SensorHistoryService service,
        ILogger<SensorHistoryJob> logger,
        ISensorRepository sensorRepository,
        IOptions<SensorHistoryOptions> options)
    {
        _service = service;
        _logger = logger;
        _sensorRepository = sensorRepository;
        _options = options.Value;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Starting sensor history sync — LookbackHours: {LookbackHours}, StepMs: {StepMs}",
                _options.LookbackHours, _options.StepMs);

            // Get sensorIds from DB
            var sensors = await _sensorRepository.GetAllAsync();
            var sensorIds = sensors.Select(s => s.SensorId).ToList();

            if (sensorIds.Count == 0)
            {
                _logger.LogWarning("No sensors found in database — skipping history sync");
                return;
            }

            // Calculate time range
            var now = DateTimeOffset.UtcNow;
            var endMs = now.ToUnixTimeMilliseconds();
            var startMs = now.AddHours(-_options.LookbackHours).ToUnixTimeMilliseconds();

            var readings = await _service.SyncSensorHistoryAsync(sensorIds, startMs, endMs, _options.StepMs, ct);

            _logger.LogInformation("Synced {Count} sensor history readings", readings.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SensorHistory job failed");
            throw;
        }
    }
}
