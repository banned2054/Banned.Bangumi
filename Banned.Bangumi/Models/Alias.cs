using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Alias
{
    [JsonPropertyName("jp")]
    public string? Jp { get; init; }

    [JsonPropertyName("kana")]
    public string? Kana { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nick")]
    public string? Nick { get; init; }

    [JsonPropertyName("romaji")]
    public string? Romaji { get; init; }

    [JsonPropertyName("zh")]
    public string? Zh { get; init; }
}
