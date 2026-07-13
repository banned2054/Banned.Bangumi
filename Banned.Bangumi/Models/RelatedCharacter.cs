using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record RelatedCharacter
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("type")]
    public int? Type { get; init; }

    /// <summary>
    /// object with some size of images, this object maybe `null`
    /// </summary>
    [JsonPropertyName("images")]
    public PersonImages? Images { get; init; }

    [JsonPropertyName("relation")]
    public string? Relation { get; init; }

    /// <summary>
    /// 演员列表
    /// </summary>
    [JsonPropertyName("actors")]
    public ICollection<Person>? Actors { get; init; }
}