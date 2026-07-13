using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Rating
{
    [JsonPropertyName("total")]
    public int? Total { get; init; }
    [JsonPropertyName("count")]
    public Count? Count { get; init; }
    /// <summary>
    /// 评分
    /// </summary>
    [JsonPropertyName("score")]
    public double? Score { get; init; }
  
}
