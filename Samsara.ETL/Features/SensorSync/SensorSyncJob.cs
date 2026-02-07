using Microsoft.Extensions.Logging;

namespace Samsara.ETL.Features.SensorSync;

public class SensorSyncJob
{
    private readonly SensorSyncService _sensorSyncService;
    private readonly ILogger<SensorSyncJob> _logger;

    public SensorSyncJob(
        SensorSyncService sensorSyncService,
        ILogger<SensorSyncJob> logger)
    {
        _sensorSyncService = sensorSyncService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting sensor sync");

        var sensors = await _sensorSyncService.SyncSensorsAsync(ct);

        _logger.LogInformation("Synced {Count} sensors:", sensors.Count);

        foreach (var sensor in sensors)
        {
            _logger.LogInformation("Sensor: {Id} - {Name} - {Mac}",
                sensor.SensorId, sensor.Name, sensor.MacAddress);
        }
    }
}