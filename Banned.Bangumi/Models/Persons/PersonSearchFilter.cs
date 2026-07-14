using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Persons;

/// <summary>
/// 表示人物搜索筛选条件。<br/>
/// Represents person search filters.
/// </summary>
public sealed record PersonSearchFilter
{
    /// <summary>
    /// 获取或初始化职业筛选值；多个职业之间为“且”关系。<br/>
    /// Gets or initializes career filters; multiple careers are combined with AND semantics.
    /// </summary>
    [JsonPropertyName("career")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<PersonCareer>? Careers { get; init; }
}
