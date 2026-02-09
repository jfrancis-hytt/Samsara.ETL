using Microsoft.Extensions.Logging;

using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Services;

namespace Samsara.ETL.Pipelines.Gateway;

public class GatewayJob
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

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting Gateway sync");

        var gateways = await _service.SyncGatewaysAsync(ct);

        _logger.LogInformation("Synced {Count} Gateways:", gateways.Count);

        foreach (var gateway in gateways)
        {
            _logger.LogInformation(
               "Gateway: {Serial} - {Model} - Health: {HealthStatus} - Asset: {AssetId}",
               gateway.Serial,
               gateway.Model,
               gateway.HealthStatus,
               gateway.AssetId);
        }
    }
}
