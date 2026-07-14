using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Episodes;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record UserEpisodeCollection
{
    [JsonPropertyName("episode")]
    public Episode? Episode { get; init; }

    [JsonPropertyName("type")]
    public EpisodeCollectionType? Type { get; init; }

    /// <summary>
    /// A int64 unix timestamp, `0` as unknown or un-recorded.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public int? UpdatedAt { get; init; }
}
