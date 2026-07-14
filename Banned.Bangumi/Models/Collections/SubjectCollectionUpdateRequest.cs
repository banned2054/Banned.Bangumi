using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示条目收藏新增或修改请求。<br/>
/// Represents a request to create or update a subject collection.
/// </summary>
public sealed record SubjectCollectionUpdateRequest
{
    /// <summary>获取或初始化收藏状态。<br/>Gets or initializes the collection status.</summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SubjectCollectionType? Type { get; init; }

    /// <summary>获取或初始化评分；<c>0</c> 表示删除评分。<br/>Gets or initializes the rating; <c>0</c> removes the rating.</summary>
    [JsonPropertyName("rate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Rate { get; init; }

    /// <summary>获取或初始化书籍条目的话数进度。<br/>Gets or initializes the chapter progress for a book subject.</summary>
    [JsonPropertyName("ep_status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? EpisodeStatus { get; init; }

    /// <summary>获取或初始化书籍条目的册数进度。<br/>Gets or initializes the volume progress for a book subject.</summary>
    [JsonPropertyName("vol_status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? VolumeStatus { get; init; }

    /// <summary>获取或初始化评价。<br/>Gets or initializes the comment.</summary>
    [JsonPropertyName("comment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; init; }

    /// <summary>获取或初始化是否仅自己可见。<br/>Gets or initializes whether the collection is private.</summary>
    [JsonPropertyName("private")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsPrivate { get; init; }

    /// <summary>获取或初始化用户标签；空集合会删除所有标签。<br/>Gets or initializes user tags; an empty collection removes all tags.</summary>
    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Tags { get; init; }
}
