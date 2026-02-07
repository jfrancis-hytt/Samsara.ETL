using Microsoft.Extensions.Logging;

namespace Samsara.ETL.Features.TrailerSync;

public class TrailerSyncJob
{
    private readonly TrailerSyncService _service;
    private readonly ILogger<TrailerSyncJob> _logger;

    public TrailerSyncJob(
        TrailerSyncService service,
        ILogger<TrailerSyncJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting trailer sync");

        var trailers = await _service.SyncTrailersAsync(ct);

        _logger.LogInformation("Synced {Count} trailers", trailers.Count);

        foreach (var trailer in trailers)
        {
            _logger.LogInformation(
                "Trailer: {Id} - {Name} - Gateway: {GatewaySerial} - License: {LicensePlate}",
                trailer.Id,
                trailer.Name,
                trailer.GatewaySerial ?? "None",
                trailer.LicensePlate ?? "N/A");
        }
    }
}
