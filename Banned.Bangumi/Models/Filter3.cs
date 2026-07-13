using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Filter3
{
    [JsonPropertyName("career")]
    public ICollection<string>? Career { get; init; }
}