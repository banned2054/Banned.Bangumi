using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record CharacterRevision : Revision
{
    [JsonPropertyName("data")]
    public Data? Data { get; init; }
}
