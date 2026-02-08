namespace Samsara.ETL.Features.SensorTemperatureSync;

public record SensorTemperatureDto(
    long SensorId,
    string Name,
    int? AmbientTemperature,
    DateTime? AmbientTemperatureTime,
    int? ProbeTemperature,
    DateTime? ProbeTemperatureTime,
    long? TrailerId
);