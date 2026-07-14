using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Index
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("desc")]
    public string? Desc { get; init; }

    /// <summary>
    /// 收录条目总数
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// 目录评论及收藏数
    /// </summary>
    [JsonPropertyName("stat")]
    public ResourceStatistics? Stat { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    [JsonPropertyName("creator")]
    public Creator? Creator { get; init; }

    /// <summary>
    /// deprecated, always false.
    /// </summary>
    [JsonPropertyName("ban")]
    [Obsolete]
    public bool? Ban { get; init; }

    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; init; }
}
