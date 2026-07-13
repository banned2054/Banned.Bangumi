using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record CharacterRevisionDataItem
{
    [JsonPropertyName("infobox")]
    public string? Infobox { get; init; }

    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("extra")]
    public RevisionExtra? Extra { get; init; }
}