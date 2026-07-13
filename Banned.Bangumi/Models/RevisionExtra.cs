using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record RevisionExtra
{
    [JsonPropertyName("img")]
    public string? Img { get; init; }
}