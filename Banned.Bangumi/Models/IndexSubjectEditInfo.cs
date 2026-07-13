using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 修改目录中条目的信息
/// </summary>
public partial record IndexSubjectEditInfo
{
    /// <summary>
    /// 排序条件，越小越靠前
    /// </summary>
    [JsonPropertyName("sort")]
    public int? Sort { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }
}