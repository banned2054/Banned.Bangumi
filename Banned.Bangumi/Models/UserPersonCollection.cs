using Banned.Bangumi.Models.Enums;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record UserPersonCollection
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// `1`, `2`, `3` 表示 `个人`, `公司`, `组合`
    /// </summary>
    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("career")]
    // TODO(system.text.json): Add ItemConverterType with enum converter when supported
    public ICollection<PersonCareer>? Career { get; init; }

    /// <summary>
    /// object with some size of images, this object maybe `null`
    /// </summary>
    [JsonPropertyName("images")]
    public PersonImages? Images { get; init; }

    [JsonPropertyName("created_at")]
    public System.DateTimeOffset? CreatedAt { get; init; }
}
