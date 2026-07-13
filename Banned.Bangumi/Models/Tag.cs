using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Tag
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("count")]
    public int? Count { get; init; }
}