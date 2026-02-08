namespace Samsara.ETL.Features.GatewaySync;

public record GatewayDto(
    string Serial,
    string Model,
    string AssetId,
    string SamsaraSerial,
    string? SamsaraVin,
    string HealthStatus,
    DateTime? LastConnected,
    long CellularDataUsageBytes,
    long HotspotUsageBytes,
    List<AccessoryDeviceDto>? AccessoryDevices
);

public record AccessoryDeviceDto(
    string Serial,
    string Model
);