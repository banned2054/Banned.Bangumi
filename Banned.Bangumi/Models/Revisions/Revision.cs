using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示修订记录的通用信息。<br/>
/// Represents common information about a revision.
/// </summary>
public record Revision
{
    /// <summary>获取修订 ID。<br/>Gets the revision ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取修订类型值。<br/>Gets the revision type value.</summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>获取修订者；API 未提供时为 <see langword="null"/>。<br/>Gets the revision creator, or <see langword="null"/> when omitted by the API.</summary>
    [JsonPropertyName("creator")]
    public Creator? Creator { get; init; }

    /// <summary>获取修订摘要。<br/>Gets the revision summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取修订创建时间。<br/>Gets when the revision was created.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }
}
