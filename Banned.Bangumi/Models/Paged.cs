using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Paged<T>
{
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("offset")]
    public int? Offset { get; init; }

    [JsonPropertyName("data")]
    public ICollection<T>? Data { get; init; }
}