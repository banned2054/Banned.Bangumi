using Banned.Bangumi.Models.Subjects;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示用户的条目收藏信息。<br/>
/// Represents a user's subject collection information.
/// </summary>
public sealed record UserSubjectCollection
{
    /// <summary>获取条目 ID。<br/>Gets the subject ID.</summary>
    [JsonPropertyName("subject_id")]
    public int SubjectId { get; init; }

    /// <summary>获取条目类型。<br/>Gets the subject type.</summary>
    [JsonPropertyName("subject_type")]
    public SubjectType SubjectType { get; init; }

    /// <summary>获取用户评分。<br/>Gets the user's rating.</summary>
    [JsonPropertyName("rate")]
    public int Rate { get; init; }

    /// <summary>获取收藏状态。<br/>Gets the collection status.</summary>
    [JsonPropertyName("type")]
    public SubjectCollectionType Type { get; init; }

    /// <summary>获取用户评价。<br/>Gets the user's comment.</summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    /// <summary>获取用户标签。<br/>Gets the user's tags.</summary>
    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; init; } = [];

    /// <summary>获取话数进度。<br/>Gets the episode or chapter progress.</summary>
    [JsonPropertyName("ep_status")]
    public int EpisodeStatus { get; init; }

    /// <summary>获取册数进度。<br/>Gets the volume progress.</summary>
    [JsonPropertyName("vol_status")]
    public int VolumeStatus { get; init; }

    /// <summary>获取收藏更新时间；服务端不会在每次信息变化时更新该值。<br/>Gets the collection update time; the server does not update this value for every information change.</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>获取收藏是否仅自己可见。<br/>Gets whether the collection is private.</summary>
    [JsonPropertyName("private")]
    public bool IsPrivate { get; init; }

    /// <summary>获取条目摘要；单项查询可能不返回该字段。<br/>Gets the subject summary, which may be omitted from a single-item response.</summary>
    [JsonPropertyName("subject")]
    public SubjectCollectionSubject? Subject { get; init; }
}
