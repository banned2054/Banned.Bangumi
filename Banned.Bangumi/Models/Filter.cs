using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Filter
{
    [JsonPropertyName("type")]
    public ICollection<SubjectType>? Type { get; init; }

    [JsonPropertyName("meta_tags")]
    public ICollection<string>? MetaTags { get; init; }

    [JsonPropertyName("tag")]
    public ICollection<string>? Tag { get; init; }

    [JsonPropertyName("air_date")]
    public ICollection<string>? AirDate { get; init; }

    [JsonPropertyName("rating")]
    public ICollection<string>? Rating { get; init; }

    [JsonPropertyName("rating_count")]
    public ICollection<string>? RatingCount { get; init; }

    [JsonPropertyName("rank")]
    public ICollection<string>? Rank { get; init; }

    [JsonPropertyName("nsfw")]
    public bool? Nsfw { get; init; }
}