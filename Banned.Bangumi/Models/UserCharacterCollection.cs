using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Persons;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record UserCharacterCollection
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
    public Images? Images { get; init; }

    [JsonPropertyName("created_at")]
    public System.DateTimeOffset? CreatedAt { get; init; }
}
