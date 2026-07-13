using Banned.Bangumi.Models.Enums;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record PersonCharacter
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    /// <summary>
    /// object with some size of images, this object maybe `null`
    /// </summary>
    [JsonPropertyName("images")]
    public PersonImages? Images { get; init; }

    [JsonPropertyName("subject_id")]
    public int? SubjectId { get; init; }

    [JsonPropertyName("subject_type")]
    public SubjectType? SubjectType { get; init; }

    [JsonPropertyName("subject_name")]
    public string? SubjectName { get; init; }

    [JsonPropertyName("subject_name_cn")]
    public string? SubjectNameCn { get; init; }

    [JsonPropertyName("staff")]
    public string? Staff { get; init; }
}