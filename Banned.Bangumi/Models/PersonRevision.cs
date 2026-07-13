using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record PersonRevision : Revision
{
    [JsonPropertyName("data")]
    public IDictionary<string, PersonRevisionDataItem>? Data { get; init; }
}
