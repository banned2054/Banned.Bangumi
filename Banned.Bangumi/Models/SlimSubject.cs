using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models;

public partial record SlimSubject
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    [JsonPropertyName("short_summary")]
    public string? ShortSummary { get; init; }

    /// <summary>
    /// air date in `YYYY-MM-DD` format
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("images")]
    public Images? Images { get; init; }

    [JsonPropertyName("volumes")]
    public int? Volumes { get; init; }

    /// <summary>
    /// 由旧服务端从wiki中解析，对于书籍条目为`话数`
    /// </summary>
    [JsonPropertyName("eps")]
    public int? Eps { get; init; }

    /// <summary>
    /// 收藏人数
    /// </summary>
    [JsonPropertyName("collection_total")]
    public int? CollectionTotal { get; init; }

    /// <summary>
    /// 分数
    /// </summary>
    [JsonPropertyName("score")]
    public double? Score { get; init; }

    /// <summary>
    /// 排名
    /// </summary>
    [JsonPropertyName("rank")]
    public int? Rank { get; init; }

    [JsonPropertyName("tags")]
    public SubjectTags? Tags { get; init; }
}
