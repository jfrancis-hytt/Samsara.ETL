using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Responses;

public record SensorResponse(
    [property: JsonPropertyName("sensors")] IReadOnlyList<Sensor> Sensors
);

public record Sensor(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("macAddress")] string MacAddress
);
