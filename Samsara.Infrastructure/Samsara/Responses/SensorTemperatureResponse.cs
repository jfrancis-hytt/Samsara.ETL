using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Responses;

public record SensorTemperatureResponse(
    [property: JsonPropertyName("groupId")] long GroupId,
    [property: JsonPropertyName("sensors")] IReadOnlyList<SensorTemperature> Sensors
);

public record SensorTemperature(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("ambientTemperature")] int? AmbientTemperature,
    [property: JsonPropertyName("ambientTemperatureTime")] DateTime? AmbientTemperatureTime,
    [property: JsonPropertyName("probeTemperature")] int? ProbeTemperature,
    [property: JsonPropertyName("probeTemperatureTime")] DateTime? ProbeTemperatureTime,
    [property: JsonPropertyName("trailerId")] long? TrailerId
);
