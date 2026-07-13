using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Stat
{
    [JsonPropertyName("comments")]
    public int? Comments { get; init; }

    [JsonPropertyName("collects")]
    public int? Collects { get; init; }
}