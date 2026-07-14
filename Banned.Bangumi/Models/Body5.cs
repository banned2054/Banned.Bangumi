using Banned.Bangumi.Models.Collections;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Body5
{
    [JsonPropertyName("type")]
    public EpisodeCollectionType? Type { get; init; }
}
