using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record DetailedRevision
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("creator")]
    public Creator? Creator { get; init; }

    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    /// 编辑修改内容，响应类型不固定
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; init; }
}