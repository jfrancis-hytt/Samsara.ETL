using Microsoft.Extensions.Logging;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.Trailer;

public class TrailerJob
{
    private readonly TrailerService _service;
    private readonly ILogger<TrailerJob> _logger;

    public TrailerJob(
        TrailerService service,
        ILogger<TrailerJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Trailer job failed");
        }


        
    }
}
