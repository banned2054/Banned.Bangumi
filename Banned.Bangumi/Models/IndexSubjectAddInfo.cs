using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record IndexSubjectAddInfo
{
    [JsonPropertyName("subject_id")]
    public int? SubjectId { get; init; }

    /// <summary>
    /// 排序条件，越小越靠前
    /// </summary>
    [JsonPropertyName("sort")]
    public int? Sort { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }
}