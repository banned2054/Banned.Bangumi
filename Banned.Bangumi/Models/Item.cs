using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Item
{
    [JsonPropertyName("key")]
    public string? Key { get; init; }

    [JsonPropertyName("value")]
    public Data? Value { get; init; }
}