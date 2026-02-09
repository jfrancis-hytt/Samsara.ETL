using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Requests;

public record SensorTemperatureRequest(
    [property: JsonPropertyName("sensors")] List<long> Sensors
);
