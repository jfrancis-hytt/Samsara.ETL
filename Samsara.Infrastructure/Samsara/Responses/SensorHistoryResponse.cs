using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Responses;

// Link: https://developers.samsara.com/reference/v1getsensorshistory

public sealed record SensorHistoryResponse(
    [property: JsonPropertyName("results")] IReadOnlyList<SensorHistoryResult> Results
);

public sealed record SensorHistoryResult(
    [property: JsonPropertyName("timeMs")] long? TimeMs,
    [property: JsonPropertyName("series")] IReadOnlyList<int?> Series
);