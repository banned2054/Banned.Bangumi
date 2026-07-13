using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record V0RelatedSubject
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public SubjectType? Type { get; init; }

    [JsonPropertyName("staff")]
    public string? Staff { get; init; }

    /// <summary>
    /// 参与章节/曲目
    /// </summary>
    [JsonPropertyName("eps")]
    public string? Eps { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; }
}