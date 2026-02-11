using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Requests;

// Link: https://developers.samsara.com/reference/v1getsensorstemperature

public record SensorTemperatureRequest(
    [property: JsonPropertyName("sensors")] List<long> Sensors
);
