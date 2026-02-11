namespace Samsara.Infrastructure.Dtos;

public record SensorHistoryDto(
    long SensorId,
    long TimeMs,
    int? ProbeTemperature,
    int? AmbientTemperature
);
