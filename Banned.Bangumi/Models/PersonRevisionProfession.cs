using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record PersonRevisionProfession
{
    [JsonPropertyName("producer")]
    public string? Producer { get; init; }

    [JsonPropertyName("mangaka")]
    public string? Mangaka { get; init; }

    [JsonPropertyName("artist")]
    public string? Artist { get; init; }

    [JsonPropertyName("seiyu")]
    public string? Seiyu { get; init; }

    [JsonPropertyName("writer")]
    public string? Writer { get; init; }

    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; init; }

    [JsonPropertyName("actor")]
    public string? Actor { get; init; }
}