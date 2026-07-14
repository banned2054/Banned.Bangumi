using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Collections;

/// <summary>
/// 表示条目收藏中包含的条目摘要。<br/>
/// Represents the subject summary included in a subject collection.
/// </summary>
public sealed record SubjectCollectionSubject
{
    /// <summary>获取条目 ID。<br/>Gets the subject ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取条目类型。<br/>Gets the subject type.</summary>
    [JsonPropertyName("type")]
    public SubjectType Type { get; init; }

    /// <summary>获取原始名称。<br/>Gets the original name.</summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>获取中文名称。<br/>Gets the Chinese name.</summary>
    [JsonPropertyName("name_cn")]
    public string NameCn { get; init; } = string.Empty;

    /// <summary>获取截短后的简介。<br/>Gets the shortened summary.</summary>
    [JsonPropertyName("short_summary")]
    public string ShortSummary { get; init; } = string.Empty;

    /// <summary>获取采用 <c>YYYY-MM-DD</c> 格式的放送或发售日期。<br/>Gets the air or release date in <c>YYYY-MM-DD</c> format.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>获取不同尺寸的条目图片。<br/>Gets subject images in different sizes.</summary>
    [JsonPropertyName("images")]
    public Images Images { get; init; } = new();

    /// <summary>获取书籍册数。<br/>Gets the number of book volumes.</summary>
    [JsonPropertyName("volumes")]
    public int Volumes { get; init; }

    /// <summary>获取由旧服务端解析的章节数；对于书籍条目表示话数。<br/>Gets the episode count parsed by the legacy server; for books, this is the chapter count.</summary>
    [JsonPropertyName("eps")]
    public int Episodes { get; init; }

    /// <summary>获取收藏人数。<br/>Gets the collection count.</summary>
    [JsonPropertyName("collection_total")]
    public int CollectionCount { get; init; }

    /// <summary>获取条目分数。<br/>Gets the subject score.</summary>
    [JsonPropertyName("score")]
    public double Score { get; init; }

    /// <summary>获取条目排名。<br/>Gets the subject rank.</summary>
    [JsonPropertyName("rank")]
    public int Rank { get; init; }

    /// <summary>获取使用次数最多的用户标签。<br/>Gets the most frequently used user tags.</summary>
    [JsonPropertyName("tags")]
    public IReadOnlyList<Tag> Tags { get; init; } = [];
}
