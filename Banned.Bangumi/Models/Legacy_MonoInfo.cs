using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

/// <summary>
/// 人物信息
/// </summary>
public partial record LegacyMonoInfo
{
    /// <summary>
    /// 生日
    /// </summary>
    [JsonPropertyName("birth")]
    public string? Birth { get; init; }
    /// <summary>
    /// 身高
    /// </summary>
    [JsonPropertyName("height")]
    public string? Height { get; init; }
    /// <summary>
    /// 性别
    /// </summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; init; }
    [JsonPropertyName("alias")]
    public Alias? Alias { get; init; }
    /// <summary>
    /// 引用来源
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; init; }
    /// <summary>
    /// 简体中文名
    /// </summary>
    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }
    /// <summary>
    /// 声优
    /// </summary>
    [JsonPropertyName("cv")]
    public string? Cv { get; init; }
    private IDictionary<string, object>? _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }
}
