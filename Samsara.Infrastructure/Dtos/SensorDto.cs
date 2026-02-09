namespace Samsara.Infrastructure.Dtos;

public record SensorDto(
    long SensorId,
    string Name,
    string MacAddress
);
