using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Body
{
    [JsonPropertyName("keyword")]
    public string? Keyword { get; init; }

    /// <summary>
    /// 排序规则
    /// <br/>
    /// <br/>- `match` meili search 的默认排序，按照匹配程度
    /// <br/>- `heat` 收藏人数
    /// <br/>- `rank` 排名由高到低
    /// <br/>- `score` 评分
    /// <br/>
    /// </summary>
    [JsonPropertyName("sort")]
    [JsonConverter(typeof(JsonStringEnumConverter<BodySort>))]
    public BodySort? Sort { get; init; }

    [JsonPropertyName("filter")]
    public Filter? Filter { get; init; }
}