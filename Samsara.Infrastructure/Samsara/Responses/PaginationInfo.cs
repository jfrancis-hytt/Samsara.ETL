using System.Text.Json.Serialization;

namespace Samsara.Infrastructure.Samsara.Responses;

public record PaginationInfo(
    [property: JsonPropertyName("endCursor")] string EndCursor,
    [property: JsonPropertyName("hasNextPage")] bool HasNextPage
);
