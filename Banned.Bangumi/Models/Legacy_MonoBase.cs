using Banned.Bangumi.Models.Common;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record LegacyMonoBase
{
    /// <summary>
    /// 人物 ID
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; init; }
    /// <summary>
    /// 人物地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }
    /// <summary>
    /// 姓名
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    /// <summary>
    /// 肖像
    /// </summary>
    [JsonPropertyName("images")]
    public Images? Images { get; init; }
    private IDictionary<string, object>? _additionalProperties;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }
}
