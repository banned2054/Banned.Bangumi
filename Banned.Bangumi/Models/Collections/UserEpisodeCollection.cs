using Banned.Bangumi.Models.Episodes;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示当前用户的章节收藏信息。<br/>
/// Represents the current user's episode collection information.
/// </summary>
public sealed record UserEpisodeCollection
{
    /// <summary>获取章节摘要。<br/>Gets the episode summary.</summary>
    [JsonPropertyName("episode")]
    public Episode Episode { get; init; } = new();

    /// <summary>获取章节收藏状态。<br/>Gets the episode collection status.</summary>
    [JsonPropertyName("type")]
    public EpisodeCollectionType Type { get; init; }

    /// <summary>获取 Unix 时间戳格式的更新时间；<c>0</c> 表示未知或未记录。<br/>Gets the update time as a Unix timestamp; <c>0</c> means unknown or unrecorded.</summary>
    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; init; }
}
