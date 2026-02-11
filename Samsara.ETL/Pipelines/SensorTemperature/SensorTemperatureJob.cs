using Microsoft.Extensions.Logging;
using Samsara.Domain.Interfaces.Repositories;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.SensorTemperature;

public class SensorTemperatureJob
{
    private readonly SensorTemperatureService _service;
    private readonly ILogger<SensorTemperatureJob> _logger;
    private readonly ISensorRepository _repository;

    public SensorTemperatureJob(
        SensorTemperatureService service,
        ILogger<SensorTemperatureJob> logger,
        ISensorRepository repository)
    {
        _service = service;
        _logger = logger;
        _repository = repository;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Starting sensor temperature sync");

            // Get sensorIds from DB
            var sensors = await _repository.GetAllAsync();
            var sensorIds = sensors.Select(s => s.SensorId).ToList();

            if (sensorIds.Count == 0)
            {
                _logger.LogWarning("No sensors found in database — skipping temperature sync");
                return;
            }

            // Inject sensorIds into service
            var temperatures = await _service.SyncSensorTemperaturesAsync(sensorIds, ct);

            _logger.LogInformation("Synced {Count} sensor temperatures:", temperatures.Count);

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
