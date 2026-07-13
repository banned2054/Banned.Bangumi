using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record SubjectRevision : Revision
{
    [JsonPropertyName("data")]
    public SubjectRevisionData? Data { get; init; }
}