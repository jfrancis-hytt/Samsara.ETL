using Microsoft.Extensions.Logging;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.SensorTemperature;

public class SensorTemperatureJob
{
    private readonly SensorTemperatureService _service;
    private readonly ILogger<SensorTemperatureJob> _logger;

    public SensorTemperatureJob(
        SensorTemperatureService service,
        ILogger<SensorTemperatureJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Starting sensor temperature sync");

            var temperatures = await _service.SyncSensorTemperaturesAsync(ct);

            _logger.LogInformation("Synced {Count} sensor temperatures", temperatures.Count);

            foreach (var temp in temperatures)
            {
                _logger.LogDebug("Temperature: {Id} - {Name} - Ambient: {Ambient} - Probe: {Probe}",
                    temp.SensorId, temp.Name, temp.AmbientTemperature, temp.ProbeTemperature);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SensorTemperature job failed");
            throw;
        }
    }
}
