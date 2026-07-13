using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 表示一页 API 结果。<br/>
/// Represents one page of API results.
/// </summary>
/// <typeparam name="T">结果项类型。 / The result item type.</typeparam>
public sealed record PagedResult<T>
{
    /// <summary>获取符合条件的结果总数。<br/>Gets the total number of matching results.</summary>
    [JsonPropertyName("total")]
    public int Total { get; init; }

    /// <summary>获取本页请求的最大结果数。<br/>Gets the maximum number of results requested for this page.</summary>
    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    /// <summary>获取本页相对于全部结果的偏移量。<br/>Gets this page's offset within all results.</summary>
    [JsonPropertyName("offset")]
    public int Offset { get; init; }

    /// <summary>获取本页的数据。<br/>Gets the data in this page.</summary>
    [JsonPropertyName("data")]
    public IReadOnlyList<T> Data { get; init; } = [];
}
