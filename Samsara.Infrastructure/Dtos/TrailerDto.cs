namespace Samsara.Infrastructure.Dtos;

public record TrailerDto(
    string Id,
    string Name,
    string? GatewaySerial,
    string? GatewayModel,
    string? SamsaraSerial,
    string? SamsaraVin,
    string? LicensePlate,
    string? Notes,
    bool EnabledForMobile,
    List<TrailerTagDto>? Tags
);

public record TrailerTagDto(
    string Id,
    string Name,
    string ParentTagId
);
