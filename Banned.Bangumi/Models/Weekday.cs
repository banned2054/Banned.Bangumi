using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Weekday
{
    [JsonPropertyName("en")]
    public string? En { get; init; }

    [JsonPropertyName("cn")]
    public string? Cn { get; init; }

    [JsonPropertyName("ja")]
    public string? Ja { get; init; }

    [JsonPropertyName("id")]
    public int? Id { get; init; }
}