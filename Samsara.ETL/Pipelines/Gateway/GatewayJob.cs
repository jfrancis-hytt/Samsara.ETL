using Microsoft.Extensions.Logging;
using Quartz;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.Gateway;

public class GatewayJob : IJob
{
    private readonly GatewayService _service;
    private readonly ILogger<GatewayJob> _logger;

    public GatewayJob(
        GatewayService service,
        ILogger<GatewayJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var ct = context.CancellationToken;
        try
        {
            _logger.LogInformation("Starting gateway sync");

            var gateways = await _service.SyncGatewaysAsync(ct);

            _logger.LogInformation("Synced {Count} gateways", gateways.Count);

            foreach (var gateway in gateways)
            {
                _logger.LogDebug(
                   "Gateway: {Serial} - {Model} - Health: {HealthStatus} - Asset: {AssetId}",
                   gateway.Serial,
                   gateway.Model,
                   gateway.HealthStatus,
                   gateway.AssetId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gateway job failed");
            throw;
        }
    }
}
