using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示 Bangumi 条目详情。<br/>
/// Represents Bangumi subject details.
/// </summary>
public sealed record Subject
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

    /// <summary>获取简介。<br/>Gets the summary.</summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; } = string.Empty;

    /// <summary>获取是否为书籍系列的主条目。<br/>Gets whether this is the main subject of a book series.</summary>
    [JsonPropertyName("series")]
    public bool Series { get; init; }

    /// <summary>获取条目是否包含 NSFW 内容。<br/>Gets whether the subject contains NSFW content.</summary>
    [JsonPropertyName("nsfw")]
    public bool Nsfw { get; init; }

    /// <summary>获取条目是否被锁定。<br/>Gets whether the subject is locked.</summary>
    [JsonPropertyName("locked")]
    public bool Locked { get; init; }

    /// <summary>
    /// 获取采用 <c>YYYY-MM-DD</c> 格式的放送或发售日期。<br/>
    /// Gets the air or release date in <c>YYYY-MM-DD</c> format.
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>获取条目平台。<br/>Gets the subject platform.</summary>
    [JsonPropertyName("platform")]
    public string Platform { get; init; } = string.Empty;

    /// <summary>获取不同尺寸的条目图片。<br/>Gets subject images in different sizes.</summary>
    [JsonPropertyName("images")]
    public Images Images { get; init; } = new();

    /// <summary>获取条目信息框；API 未提供时为 <see langword="null"/>。<br/>Gets the subject infobox, or <see langword="null"/> when the API does not provide one.</summary>
    [JsonPropertyName("infobox")]
    public IReadOnlyList<SubjectInfoboxItem>? Infobox { get; init; }

    /// <summary>获取书籍册数。<br/>Gets the number of book volumes.</summary>
    [JsonPropertyName("volumes")]
    public int Volumes { get; init; }

    /// <summary>
    /// 获取由旧服务端从 Wiki 解析的章节数；对于书籍条目表示话数。<br/>
    /// Gets the episode count parsed from the wiki by the legacy server; for books, this is the chapter count.
    /// </summary>
    [JsonPropertyName("eps")]
    public int Episodes { get; init; }

    /// <summary>获取数据库中的章节总数。<br/>Gets the total number of episodes in the database.</summary>
    [JsonPropertyName("total_episodes")]
    public int TotalEpisodes { get; init; }

    /// <summary>获取条目评分信息。<br/>Gets subject rating information.</summary>
    [JsonPropertyName("rating")]
    public SubjectRating Rating { get; init; } = new();

    /// <summary>获取各收藏状态的人数。<br/>Gets user counts by collection status.</summary>
    [JsonPropertyName("collection")]
    public SubjectCollectionStats Collection { get; init; } = new();

    /// <summary>获取由 Wiki 维护的公共标签。<br/>Gets meta tags maintained by the wiki.</summary>
    [JsonPropertyName("meta_tags")]
    public IReadOnlyList<string> MetaTags { get; init; } = [];

    /// <summary>获取用户标签及其使用次数。<br/>Gets user tags and their usage counts.</summary>
    [JsonPropertyName("tags")]
    public IReadOnlyList<Tag> Tags { get; init; } = [];
}
