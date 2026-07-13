using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Anonymous
{
    [JsonPropertyName("weekday")]
    public Weekday? Weekday { get; init; }

    [JsonPropertyName("items")]
    public ICollection<LegacySubjectSmall>? Items { get; init; }
}