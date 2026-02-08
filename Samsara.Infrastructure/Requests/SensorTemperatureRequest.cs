using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Requests;

public record SensorTemperatureRequest(
    [property: JsonPropertyName("sensors")] List<long> Sensors
);
