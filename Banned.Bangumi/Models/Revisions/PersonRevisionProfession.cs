using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示人物修订中的职业标记。<br/>
/// Represents profession flags in a person revision.
/// </summary>
public sealed record PersonRevisionProfession
{
    /// <summary>获取制片人标记。<br/>Gets the producer flag.</summary>
    [JsonPropertyName("producer")]
    public string? Producer { get; init; }

    /// <summary>获取漫画家标记。<br/>Gets the manga artist flag.</summary>
    [JsonPropertyName("mangaka")]
    public string? MangaArtist { get; init; }

    /// <summary>获取艺术家标记。<br/>Gets the artist flag.</summary>
    [JsonPropertyName("artist")]
    public string? Artist { get; init; }

    /// <summary>获取声优标记。<br/>Gets the voice actor flag.</summary>
    [JsonPropertyName("seiyu")]
    public string? VoiceActor { get; init; }

    /// <summary>获取作家标记。<br/>Gets the writer flag.</summary>
    [JsonPropertyName("writer")]
    public string? Writer { get; init; }

    /// <summary>获取插画师标记。<br/>Gets the illustrator flag.</summary>
    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; init; }

    /// <summary>获取演员标记。<br/>Gets the actor flag.</summary>
    [JsonPropertyName("actor")]
    public string? Actor { get; init; }
}
