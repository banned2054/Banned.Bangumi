using Banned.Bangumi.Models.Enums;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record PersonDetail
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

    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("locked")]
    public bool? Locked { get; init; }

    /// <summary>
    /// currently it's latest user comment time, it will be replaced by wiki modified date in the future
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTimeOffset? LastModified { get; init; }

    /// <summary>
    /// server parsed infobox, a map from key to string or tuple
    /// <br/>null if server infobox is not valid
    /// </summary>
    [JsonPropertyName("infobox")]
    public ICollection<object>? Infobox { get; init; }

    /// <summary>
    /// parsed from wiki, maybe null
    /// </summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; init; }

    /// <summary>
    /// parsed from wiki, maybe null, `1, 2, 3, 4` for `A, B, AB, O`
    /// </summary>
    [JsonPropertyName("blood_type")]
    public int? BloodType { get; init; }

    /// <summary>
    /// parsed from wiki, maybe `null`
    /// </summary>
    [JsonPropertyName("birth_year")]
    public int? BirthYear { get; init; }

    /// <summary>
    /// parsed from wiki, maybe `null`
    /// </summary>
    [JsonPropertyName("birth_mon")]
    public int? BirthMon { get; init; }

    /// <summary>
    /// parsed from wiki, maybe `null`
    /// </summary>
    [JsonPropertyName("birth_day")]
    public int? BirthDay { get; init; }

    [JsonPropertyName("stat")]
    public Stat? Stat { get; init; }
}
