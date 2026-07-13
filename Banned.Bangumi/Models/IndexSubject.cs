using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models;

public partial record IndexSubject
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("images")]
    public SubjectImages? Images { get; init; }

    [JsonPropertyName("infobox")]
    public WikiV0? Infobox { get; init; }

    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("added_at")]
    public DateTimeOffset? AddedAt { get; init; }
}
