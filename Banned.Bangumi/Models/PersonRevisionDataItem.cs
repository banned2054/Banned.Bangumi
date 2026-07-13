using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record PersonRevisionDataItem
{
    [JsonPropertyName("prsn_infobox")]
    public string? Infobox { get; init; }

    [JsonPropertyName("prsn_summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("profession")]
    public PersonRevisionProfession? Profession { get; init; }

    [JsonPropertyName("extra")]
    public RevisionExtra? Extra { get; init; }

    [JsonPropertyName("prsn_name")]
    public string? Name { get; init; }
}