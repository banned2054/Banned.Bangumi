using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record SubjectRevisionData
{
    [JsonPropertyName("field_eps")]
    public int? FieldEps { get; init; }

    [JsonPropertyName("field_infobox")]
    public string? FieldInfobox { get; init; }

    [JsonPropertyName("field_summary")]
    public string? FieldSummary { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("name_cn")]
    public string? NameCn { get; init; }

    [JsonPropertyName("platform")]
    public int? Platform { get; init; }

    [JsonPropertyName("subject_id")]
    public int? SubjectId { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("type_id")]
    public int? TypeId { get; init; }

    [JsonPropertyName("vote_field")]
    public string? VoteField { get; init; }
}