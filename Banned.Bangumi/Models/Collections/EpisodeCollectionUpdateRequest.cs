using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示修改单个章节收藏状态的请求。<br/>
/// Represents a request to update a single episode collection status.
/// </summary>
public sealed record EpisodeCollectionUpdateRequest
{
    /// <summary>获取或初始化目标收藏状态。<br/>Gets or initializes the target collection status.</summary>
    [JsonPropertyName("type")]
    public required EpisodeCollectionType Type { get; init; }
}
