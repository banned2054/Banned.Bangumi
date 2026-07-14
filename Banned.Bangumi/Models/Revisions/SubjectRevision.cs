using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示条目修订详情。<br/>
/// Represents subject revision details.
/// </summary>
public sealed record SubjectRevision : Revision
{
    /// <summary>获取条目修订数据；API 未提供时为 <see langword="null"/>。<br/>Gets subject revision data, or <see langword="null"/> when omitted by the API.</summary>
    [JsonPropertyName("data")]
    public SubjectRevisionData? Data { get; init; }
}
