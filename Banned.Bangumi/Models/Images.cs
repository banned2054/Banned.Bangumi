using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Images
{
    [JsonPropertyName("large")]
    public string? Large { get; init; }

    [JsonPropertyName("common")]
    public string? Common { get; init; }

    [JsonPropertyName("medium")]
    public string? Medium { get; init; }

    [JsonPropertyName("small")]
    public string? Small { get; init; }

    [JsonPropertyName("grid")]
    public string? Grid { get; init; }
}