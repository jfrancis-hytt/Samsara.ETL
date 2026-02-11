using Microsoft.Extensions.Logging;

using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.Sensor;

public class SensorJob
{
    private readonly SensorService _sensorSyncService;
    private readonly ILogger<SensorJob> _logger;

    public SensorJob(
        SensorService sensorSyncService,
        ILogger<SensorJob> logger)
    {
        _sensorSyncService = sensorSyncService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sensor job failed");
            throw;
        }

       
    }
}