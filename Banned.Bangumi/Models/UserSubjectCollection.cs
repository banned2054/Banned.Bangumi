using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Subjects;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record UserSubjectCollection
{
    [JsonPropertyName("subject_id")]
    public int? SubjectId { get; init; }

    [JsonPropertyName("subject_type")]
    public SubjectType? SubjectType { get; init; }

    [JsonPropertyName("rate")]
    public int? Rate { get; init; }

    [JsonPropertyName("type")]
    public SubjectCollectionType? Type { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("tags")]
    public ICollection<string>? Tags { get; init; }

    [JsonPropertyName("ep_status")]
    public int? EpStatus { get; init; }

    [JsonPropertyName("vol_status")]
    public int? VolStatus { get; init; }

    [JsonPropertyName("updated_at")]
    public System.DateTimeOffset? UpdatedAt { get; init; }

    [JsonPropertyName("private")]
    public bool? Private { get; init; }

    [JsonPropertyName("subject")]
    public SlimSubject? Subject { get; init; }
}
