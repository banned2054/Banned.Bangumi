using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models;

public partial record V0SubjectRelation
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    [JsonPropertyName("images")]
    public SubjectImages? Images { get; init; }

    [JsonPropertyName("relation")]
    public string? Relation { get; init; }
}
