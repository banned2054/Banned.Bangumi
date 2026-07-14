using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示人物修订详情。<br/>
/// Represents person revision details.
/// </summary>
public sealed record PersonRevision : Revision
{
    /// <summary>获取以服务端字段标识为键的修订数据。<br/>Gets revision data keyed by server-side field identifiers.</summary>
    [JsonPropertyName("data")]
    public IReadOnlyDictionary<string, PersonRevisionDataItem> Data { get; init; } =
        new Dictionary<string, PersonRevisionDataItem>();
}
