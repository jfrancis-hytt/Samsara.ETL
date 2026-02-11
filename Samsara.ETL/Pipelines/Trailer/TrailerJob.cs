using Microsoft.Extensions.Logging;
using Quartz;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.Trailer;

public class TrailerJob : IJob
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

    public async Task Execute(IJobExecutionContext context)
    {
        var ct = context.CancellationToken;
        try
        {
            _logger.LogInformation("Starting trailer sync");

            var trailers = await _service.SyncTrailersAsync(ct);

            _logger.LogInformation("Synced {Count} trailers", trailers.Count);

            foreach (var trailer in trailers)
            {
                _logger.LogDebug(
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
            throw;
        }
    }
}
