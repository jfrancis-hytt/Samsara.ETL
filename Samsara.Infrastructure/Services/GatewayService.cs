using Samsara.Domain.Entities;
using Samsara.Domain.Repositories;
using Samsara.Infrastructure.Dtos;
using Samsara.Infrastructure.Samsara.Client;

namespace Samsara.Infrastructure.Services;

public class GatewayService
{
    private readonly ISamsaraClient _samsaraClient;
    private readonly IGatewayRepository _gatewayRepository;
    private readonly IAccessoryDeviceRepository _accessoryDeviceRepository;

    public GatewayService(
        ISamsaraClient samsaraClient,
        IGatewayRepository gatewayRepository,
        IAccessoryDeviceRepository accessoryDeviceRepository)
    {
        _samsaraClient = samsaraClient;
        _gatewayRepository = gatewayRepository;
        _accessoryDeviceRepository = accessoryDeviceRepository;
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

        foreach (var dto in gatewayDtos)
        {
            var entity = new Gateway
            {
                Serial = dto.Serial,
                Model = dto.Model,
                AssetId = dto.AssetId,
                SamsaraSerial = dto.SamsaraSerial,
                SamsaraVin = dto.SamsaraVin,
                HealthStatus = dto.HealthStatus,
                LastConnected = dto.LastConnected,
                CellularDataUsageBytes = dto.CellularDataUsageBytes,
                HotspotUsageBytes = dto.HotspotUsageBytes
            };
            await _gatewayRepository.UpsertAsync(entity);

            var accessories = dto.AccessoryDevices?.Select(a => new AccessoryDevice
            {
                Serial = a.Serial,
                Model = a.Model,
                GatewaySerial = dto.Serial
            }) ?? [];
            await _accessoryDeviceRepository.ReplaceByGatewayAsync(dto.Serial, accessories);
        }

        return gatewayDtos;
    }
}
