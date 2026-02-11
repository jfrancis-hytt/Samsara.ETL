using Samsara.Domain.Entities;
using Samsara.Domain.Interfaces.Repositories;
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
            AssetId: g.Asset?.Id ?? string.Empty,
            SamsaraSerial: g.Asset?.ExternalIds?.SamsaraSerial ?? string.Empty,
            SamsaraVin: g.Asset?.ExternalIds?.SamsaraVin ?? null,
            HealthStatus: g.ConnectionStatus?.HealthStatus ?? string.Empty,
            LastConnected: g.ConnectionStatus?.LastConnected ?? null,
            CellularDataUsageBytes: g.DataUsageLast30Days?.CellularDataUsageBytes ?? 0,
            HotspotUsageBytes: g.DataUsageLast30Days?.HotspotUsageBytes ?? 0,
            AccessoryDevices: g.AccessoryDevices?.Select(a => new AccessoryDeviceDto(
                Serial: a.Serial,
                Model: a.Model
            )).ToList()
        )).ToList();

        var entities = gatewayDtos.Select(dto => new GatewayEntity
        {
            Serial = dto.Serial,
            Model = dto.Model,
            AssetId = dto.AssetId ?? string.Empty,
            SamsaraSerial = dto.SamsaraSerial ?? string.Empty,
            SamsaraVin = dto.SamsaraVin,
            HealthStatus = dto.HealthStatus ?? string.Empty,
            LastConnected = dto.LastConnected,
            CellularDataUsageBytes = dto.CellularDataUsageBytes ?? 0,
            HotspotUsageBytes = dto.HotspotUsageBytes ?? 0
        });
        await _gatewayRepository.UpsertBatchAsync(entities);

        var allAccessories = gatewayDtos.SelectMany(dto =>
            dto.AccessoryDevices?.Select(a => new AccessoryDeviceEntity
            {
                Serial = a.Serial ?? string.Empty,
                Model = a.Model ?? string.Empty,
                GatewaySerial = dto.Serial
            }) ?? []);
        await _accessoryDeviceRepository.ReplaceAllAsync(allAccessories);

        return gatewayDtos;
    }
}
