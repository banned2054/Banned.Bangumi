using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Indices;

/// <summary>
/// 表示目录中的条目。<br/>
/// Represents a subject in an index.
/// </summary>
public sealed record IndexSubject
{
    /// <summary>获取条目 ID。<br/>Gets the subject ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取条目类型。<br/>Gets the subject type.</summary>
    [JsonPropertyName("type")]
    public SubjectType Type { get; init; }

    /// <summary>获取条目名称。<br/>Gets the subject name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取不同尺寸的条目图片；API 未提供时为 <see langword="null"/>。<br/>Gets subject images in different sizes, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    /// <summary>获取条目信息框；API 未提供时为 <see langword="null"/>。<br/>Gets the subject infobox, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("infobox")]
    public IReadOnlyList<SubjectInfoboxItem>? Infobox { get; init; }

    /// <summary>获取采用 <c>YYYY-MM-DD</c> 格式的放送或发售日期；API 未提供时为 <see langword="null"/>。<br/>Gets the air or release date in <c>YYYY-MM-DD</c> format, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>获取目录条目备注。<br/>Gets the index subject comment.</summary>
    [JsonPropertyName("comment")]
    public string Comment { get; init; } = string.Empty;

    /// <summary>获取条目加入目录的时间。<br/>Gets the time when the subject was added to the index.</summary>
    [JsonPropertyName("added_at")]
    public DateTimeOffset AddedAt { get; init; }
}
