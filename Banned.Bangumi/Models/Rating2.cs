using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Rating2
{
    [JsonPropertyName("rank")]
    public int? Rank { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("count")]
    public Count? Count { get; init; }

    [JsonPropertyName("score")]
    public double? Score { get; init; }
}