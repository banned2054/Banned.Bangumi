using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record LegacySubjectSmall
{
    /// <summary>
    /// 条目 ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }
    /// <summary>
    /// 条目地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
    [JsonPropertyName("type")]
    public LegacySubjectSmallType? Type { get; init; }
    /// <summary>
    /// 条目名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    /// <summary>
    /// 条目中文名称
    /// </summary>
    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }
    [JsonPropertyName("air_date")]
    public string? AirDate { get; init; }
    [JsonPropertyName("air_weekday")]
    public int? AirWeekday { get; init; }
    /// <summary>
    /// 封面
    /// </summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }
    /// <summary>
    /// 话数
    /// </summary>
    [JsonPropertyName("eps")]
    public int? Eps { get; init; }
    /// <summary>
    /// 话数
    /// </summary>
    [JsonPropertyName("eps_count")]
    public int? EpsCount { get; init; }
    /// <summary>
    /// 评分
    /// </summary>
    [JsonPropertyName("rating")]
    public Rating? Rating { get; init; }
    /// <summary>
    /// 排名
    /// </summary>
    [JsonPropertyName("rank")]
    public int? Rank { get; init; }
    /// <summary>
    /// 收藏人数
    /// </summary>
    [JsonPropertyName("collection")]
    public Collection? Collection { get; init; }
    private IDictionary<string, object>? _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }
}
