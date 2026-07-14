using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Indices;

/// <summary>
/// 表示 Bangumi 目录。<br/>
/// Represents a Bangumi index.
/// </summary>
public sealed record BangumiIndex
{
    /// <summary>获取目录 ID。<br/>Gets the index ID.</summary>
    [JsonPropertyName("id")]
    public int Id { get; init; }

    /// <summary>获取目录标题。<br/>Gets the index title.</summary>
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    /// <summary>获取目录描述。<br/>Gets the index description.</summary>
    [JsonPropertyName("desc")]
    public string Description { get; init; } = string.Empty;

    /// <summary>获取目录收录的条目总数。<br/>Gets the total number of subjects in the index.</summary>
    [JsonPropertyName("total")]
    public int Total { get; init; }

    /// <summary>获取目录的评论和收藏统计。<br/>Gets comment and collection statistics for the index.</summary>
    [JsonPropertyName("stat")]
    public ResourceStatistics Statistics { get; init; } = new();

    /// <summary>获取目录创建时间。<br/>Gets the time when the index was created.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>获取目录更新时间。<br/>Gets the time when the index was updated.</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>获取目录创建者。<br/>Gets the index creator.</summary>
    [JsonPropertyName("creator")]
    public Creator Creator { get; init; } = new();

    /// <summary>
    /// 获取已弃用的封禁标记；服务端始终返回 <see langword="false"/>。<br/>
    /// Gets the deprecated ban flag; the server always returns <see langword="false"/>.
    /// </summary>
    [JsonPropertyName("ban")]
    [Obsolete("Bangumi always returns false for this deprecated field.")]
    public bool Ban { get; init; }

    /// <summary>获取目录是否包含 NSFW 条目。<br/>Gets whether the index contains NSFW subjects.</summary>
    [JsonPropertyName("nsfw")]
    public bool Nsfw { get; init; }
}
