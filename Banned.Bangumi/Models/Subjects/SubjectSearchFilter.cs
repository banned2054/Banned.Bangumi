using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Enums;

namespace Banned.Bangumi.Models.Subjects;

/// <summary>
/// 表示条目搜索筛选条件；不同属性之间以及同一数组中的多个值之间均为“且”关系。<br/>
/// Represents subject search filters; different properties and multiple values in the same array use AND semantics.
/// </summary>
public sealed record SubjectSearchFilter
{
    /// <summary>获取或初始化条目类型；多个类型之间为“或”关系。<br/>Gets or initializes subject types; multiple types use OR semantics.</summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<SubjectType>? Types { get; init; }

    /// <summary>获取或初始化公共标签表达式。<br/>Gets or initializes meta-tag expressions.</summary>
    [JsonPropertyName("meta_tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? MetaTags { get; init; }

    /// <summary>获取或初始化用户标签表达式。<br/>Gets or initializes user-tag expressions.</summary>
    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Tags { get; init; }

    /// <summary>获取或初始化采用 <c>YYYY-MM-DD</c> 日期的比较表达式。<br/>Gets or initializes comparison expressions using <c>YYYY-MM-DD</c> dates.</summary>
    [JsonPropertyName("air_date")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? AirDates { get; init; }

    /// <summary>获取或初始化评分比较表达式。<br/>Gets or initializes rating comparison expressions.</summary>
    [JsonPropertyName("rating")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Ratings { get; init; }

    /// <summary>获取或初始化评分人数比较表达式。<br/>Gets or initializes rating-count comparison expressions.</summary>
    [JsonPropertyName("rating_count")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? RatingCounts { get; init; }

    /// <summary>获取或初始化排名比较表达式。<br/>Gets or initializes rank comparison expressions.</summary>
    [JsonPropertyName("rank")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Ranks { get; init; }

    /// <summary>获取或初始化 NSFW 筛选值。<br/>Gets or initializes the NSFW filter.</summary>
    [JsonPropertyName("nsfw")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Nsfw { get; init; }
}
