using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi;

public partial record EpisodeDetail
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }
    [JsonPropertyName("type")]
    public EpType? Type { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }
    /// <summary>
    /// 同类条目的排序和集数
    /// </summary>
    [JsonPropertyName("sort")]
    public double? Sort { get; init; }
    [JsonPropertyName("ep")]
    public double? Ep { get; init; }
    [JsonPropertyName("airdate")]
    public string? Airdate { get; init; }
    [JsonPropertyName("comment")]
    public int? Comment { get; init; }
    [JsonPropertyName("duration")]
    public string? Duration { get; init; }
    [JsonPropertyName("desc")]
    public string? Desc { get; init; }
    /// <summary>
    /// 音乐曲目的碟片数
    /// </summary>
    [JsonPropertyName("disc")]
    public int? Disc { get; init; }
    [JsonPropertyName("subject_id")]
    public int? SubjectId { get; init; }
    private IDictionary<string, object>? _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }
}
