using Banned.Bangumi.Models.Collections;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record UserSubjectCollectionModifyPayload
{
    /// <summary>
    /// 修改条目收藏类型
    /// </summary>
    [JsonPropertyName("type")]
    public SubjectCollectionType? Type { get; init; }

    /// <summary>
    /// 评分，`0` 表示删除评分
    /// </summary>
    [JsonPropertyName("rate")]
    public int? Rate { get; init; }

    /// <summary>
    /// 只能用于修改书籍条目进度
    /// </summary>
    [JsonPropertyName("ep_status")]
    public int? EpStatus { get; init; }

    /// <summary>
    /// 只能用于修改书籍条目进度
    /// </summary>
    [JsonPropertyName("vol_status")]
    public int? VolStatus { get; init; }

    /// <summary>
    /// 评价
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("private")]
    public bool? Private { get; init; }

    [JsonPropertyName("tags")]
    public ICollection<string>? Tags { get; init; }
}
