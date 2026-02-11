using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Responses;

// Link: https://developers.samsara.com/reference/listtrailers
public record TrailerResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<Trailer> Data,
    [property: JsonPropertyName("pagination")] PaginationInfo Pagination
);

public record Trailer(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("tags")] List<TrailerTag>? Tags,
    [property: JsonPropertyName("installedGateway")] InstalledGateway? InstalledGateway,
    [property: JsonPropertyName("externalIds")] TrailerExternalIds? ExternalIds,
    [property: JsonPropertyName("licensePlate")] string? LicensePlate,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("enabledForMobile")] bool? EnabledForMobile
);

public record TrailerTag(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("parentTagId")] string? ParentTagId
);

public record InstalledGateway(
    [property: JsonPropertyName("serial")] string Serial,
    [property: JsonPropertyName("model")] string Model
);

public record TrailerExternalIds(
    [property: JsonPropertyName("samsara.serial")] string? SamsaraSerial,
    [property: JsonPropertyName("samsara.vin")] string? SamsaraVin
);
