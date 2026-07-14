using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示条目修订的数据。<br/>
/// Represents data in a subject revision.
/// </summary>
public sealed record SubjectRevisionData
{
    /// <summary>获取章节数字段值。<br/>Gets the episode-count field value.</summary>
    [JsonPropertyName("field_eps")]
    public int EpisodeCount { get; init; }

    /// <summary>获取信息框的 Wiki 源文本。<br/>Gets the wiki source text for the infobox.</summary>
    [JsonPropertyName("field_infobox")]
    public string Infobox { get; init; } = string.Empty;

    /// <summary>获取简介字段值。<br/>Gets the summary field value.</summary>
    [JsonPropertyName("field_summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取条目原始名称。<br/>Gets the original subject name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取条目中文名称。<br/>Gets the Chinese subject name.</summary>
    [JsonPropertyName("name_cn")]
    public string NameCn { get; init; } = string.Empty;

    /// <summary>获取平台标识值。<br/>Gets the platform identifier value.</summary>
    [JsonPropertyName("platform")]
    public int Platform { get; init; }

    /// <summary>获取条目 ID。<br/>Gets the subject ID.</summary>
    [JsonPropertyName("subject_id")]
    public int SubjectId { get; init; }

    /// <summary>获取修订类型值。<br/>Gets the revision type value.</summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>获取条目类型标识值。<br/>Gets the subject type identifier value.</summary>
    [JsonPropertyName("type_id")]
    public int TypeId { get; init; }

    /// <summary>获取投票字段值。<br/>Gets the vote field value.</summary>
    [JsonPropertyName("vote_field")]
    public string VoteField { get; init; } = string.Empty;
}
