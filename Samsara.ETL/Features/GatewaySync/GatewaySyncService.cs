using Samsara.Infrastructure.Client;
using Samsara.Infrastructure.Responses;

namespace Samsara.ETL.Features.GatewaySync;

public class GatewaySyncService
{
    private readonly ISamsaraClient _samsaraClient;

    public GatewaySyncService(ISamsaraClient samsaraClient)
    {
        _samsaraClient = samsaraClient;
    }

    public async Task<List<GatewayDto>> SyncGatewaysAsync(CancellationToken ct = default)
    {
        var response = await _samsaraClient.GetGatewaysAsync(ct);

        var gatewayDtos = response.Data.Select(g => new GatewayDto(
            Serial: g.Serial,
            Model: g.Model,
            AssetId: g.Asset.Id,
            SamsaraSerial: g.Asset.ExternalIds.SamsaraSerial,
            SamsaraVin: g.Asset.ExternalIds.SamsaraVin,
            HealthStatus: g.ConnectionStatus.HealthStatus,
            LastConnected: g.ConnectionStatus.LastConnected,
            CellularDataUsageBytes: g.DataUsageLast30Days.CellularDataUsageBytes,
            HotspotUsageBytes: g.DataUsageLast30Days.HotspotUsageBytes,
            AccessoryDevices: g.AccessoryDevices?.Select(a => new AccessoryDeviceDto(
                Serial: a.Serial,
                Model: a.Model
            )).ToList()
        )).ToList();

        //TODO: Map to database models and save to database

        return gatewayDtos;
    }
}