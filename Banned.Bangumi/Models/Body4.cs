using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public record Body4
{
    [JsonPropertyName("episode_id")]
    public ICollection<int>? EpisodeId { get; init; }

    [JsonPropertyName("type")]
    public EpisodeCollectionType? Type { get; init; }
}