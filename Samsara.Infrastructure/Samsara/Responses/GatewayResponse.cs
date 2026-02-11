using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Responses;

// Link: https://developers.samsara.com/reference/getgateways

public record GatewayResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<Gateway> Data,
    [property: JsonPropertyName("pagination")] PaginationInfo Pagination
);

public record Gateway(
    [property: JsonPropertyName("serial")] string Serial,
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("asset")] AssetInfo Asset,
    [property: JsonPropertyName("connectionStatus")] ConnectionStatus ConnectionStatus,
    [property: JsonPropertyName("dataUsageLast30Days")] DataUsage DataUsageLast30Days,
    [property: JsonPropertyName("accessoryDevices")] List<AccessoryDevice>? AccessoryDevices
);

public record AssetInfo(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("externalIds")] ExternalIds ExternalIds
);

public record ExternalIds(
    [property: JsonPropertyName("samsara.serial")] string? SamsaraSerial,
    [property: JsonPropertyName("samsara.vin")] string? SamsaraVin
);

public record ConnectionStatus(
    [property: JsonPropertyName("healthStatus")] string? HealthStatus,
    [property: JsonPropertyName("lastConnected")] DateTime? LastConnected
);

public record DataUsage(
    [property: JsonPropertyName("cellularDataUsageBytes")] long? CellularDataUsageBytes,
    [property: JsonPropertyName("hotspotUsageBytes")] long? HotspotUsageBytes
);

public record AccessoryDevice(
    [property: JsonPropertyName("serial")] string? Serial,
    [property: JsonPropertyName("model")] string? Model
);

public record PaginationInfo(
    [property: JsonPropertyName("endCursor")] string EndCursor,
    [property: JsonPropertyName("hasNextPage")] bool HasNextPage
);
