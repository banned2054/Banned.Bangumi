using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Creator
{
    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; init; }
}