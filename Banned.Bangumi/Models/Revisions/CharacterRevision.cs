using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示角色修订详情。<br/>
/// Represents character revision details.
/// </summary>
public sealed record CharacterRevision : Revision
{
    /// <summary>获取以服务端字段标识为键的修订数据。<br/>Gets revision data keyed by server-side field identifiers.</summary>
    [JsonPropertyName("data")]
    public IReadOnlyDictionary<string, CharacterRevisionDataItem> Data { get; init; } =
        new Dictionary<string, CharacterRevisionDataItem>();
}
