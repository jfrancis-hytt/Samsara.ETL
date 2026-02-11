using Microsoft.Extensions.Logging;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.SensorHistory;

public class SensorHistoryJob
{
    private readonly SensorHistoryService _service;
    private readonly ILogger<SensorHistoryJob> _logger;

    public SensorHistoryJob(
        SensorHistoryService service,
        ILogger<SensorHistoryJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Starting sensor history sync");

            var readings = await _service.SyncSensorHistoryAsync(ct);

            _logger.LogInformation("Synced {Count} sensor history readings", readings.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SensorHistory job failed");
            throw;
        }
    }
}
