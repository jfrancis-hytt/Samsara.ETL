namespace Samsara.ETL.Features.SensorSync;

public record SensorDto(
    long SensorId,
    string Name,
    string MacAddress
);