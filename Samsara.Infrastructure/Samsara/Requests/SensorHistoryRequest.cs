using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Requests;

// Link: https://developers.samsara.com/reference/v1getsensorshistory

public sealed record SensorHistoryRequest(
    [property: JsonPropertyName("fillMissing")] string FillMissing,
    [property: JsonPropertyName("series")] IReadOnlyList<SensorHistorySeries> Series,
    [property: JsonPropertyName("stepMs")] long StepMs,
    [property: JsonPropertyName("startMs")] long StartMs,
    [property: JsonPropertyName("endMs")] long EndMs
);

public sealed record SensorHistorySeries(
    [property: JsonPropertyName("field")] string Field, // probeTemperature or ambientTemperature (order of this matters for response)
    [property: JsonPropertyName("widgetId")] string WidgetId // sensorId
);

