using Microsoft.Extensions.Logging;

namespace Samsara.ETL.Features.SensorTemperatureSync;

public class SensorTemperatureSyncJob
{
    private readonly SensorTemperatureSyncService _service;
    private readonly ILogger<SensorTemperatureSyncJob> _logger;

    public SensorTemperatureSyncJob(
        SensorTemperatureSyncService service,
        ILogger<SensorTemperatureSyncJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting sensor temperature sync");

        var sensorData = new List<long> {   // This is only for testing. Later we will need to refresh this list from DB
                278018081898470,
                278018081898612,
                278018084936224,
                278018084936251,
                278018086955047,
                278018088426101,
                278018087768492,
                278018088426499,
                278018087768495,
                278018087349668,
                278018087349658,
                278018087349667,
                278018081728043,
                278018087760317,
                278018087768503,
                278018086955050,
                278018087349669,
                278018087349666,
                278018087768513,
                281474998936609
        };

        var temperatures = await _service.SyncSensorTemperaturesAsync(sensorData, ct);

        _logger.LogInformation("Synced {Count} sensor temperatures:", temperatures.Count);

        foreach (var temp in temperatures)
        {
            _logger.LogInformation("Temperature: {Id} - {Name} - Ambient: {Ambient} - Probe: {Probe}",
                temp.SensorId, temp.Name, temp.AmbientTemperature, temp.ProbeTemperature);
        }
    }
}
