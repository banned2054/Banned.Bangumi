using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Characters;

/// <summary>
/// 表示角色搜索请求。<br/>
/// Represents a character search request.
/// </summary>
public sealed record CharacterSearchRequest
{
    /// <summary>获取或初始化搜索关键字。<br/>Gets or initializes the search keyword.</summary>
    [JsonPropertyName("keyword")]
    public required string Keyword { get; init; }

    /// <summary>获取或初始化搜索筛选条件。<br/>Gets or initializes the search filters.</summary>
    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CharacterSearchFilter? Filter { get; init; }

    /// <summary>获取或初始化返回数量。<br/>Gets or initializes the result limit.</summary>
    [JsonIgnore]
    public int? Limit { get; init; }

    /// <summary>获取或初始化分页偏移量。<br/>Gets or initializes the pagination offset.</summary>
    [JsonIgnore]
    public int? Offset { get; init; }
}
