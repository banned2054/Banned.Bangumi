using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示批量修改章节收藏状态的请求。<br/>
/// Represents a request to update episode collection statuses in a batch.
/// </summary>
public sealed record EpisodeCollectionBatchUpdateRequest
{
    /// <summary>获取或初始化要修改的章节 ID。<br/>Gets or initializes the episode IDs to update.</summary>
    [JsonPropertyName("episode_id")]
    public required IReadOnlyList<int> EpisodeIds { get; init; }

    /// <summary>获取或初始化目标收藏状态。<br/>Gets or initializes the target collection status.</summary>
    [JsonPropertyName("type")]
    public required EpisodeCollectionType Type { get; init; }
}
