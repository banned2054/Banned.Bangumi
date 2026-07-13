using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Avatar
{
    [JsonPropertyName("large")]
    public string? Large { get; init; }

    [JsonPropertyName("medium")]
    public string? Medium { get; init; }

    [JsonPropertyName("small")]
    public string? Small { get; init; }
}