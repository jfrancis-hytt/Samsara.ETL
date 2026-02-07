using Microsoft.Extensions.Logging;

namespace Samsara.ETL.Features.GatewaySync;

public class GatewaySyncJob
{
    private readonly GatewaySyncService _service;
    private readonly ILogger<GatewaySyncJob> _logger;

    public GatewaySyncJob(
        GatewaySyncService service,
        ILogger<GatewaySyncJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting sensor sync");

        var gateways = await _service.SyncGatewaysAsync(ct);

        _logger.LogInformation("Synced {Count} sensors:", gateways.Count);

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
