using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Revisions;

/// <summary>
/// 表示章节修订详情。<br/>
/// Represents episode revision details.
/// </summary>
public sealed record EpisodeRevision : Revision
{
    /// <summary>
    /// 获取结构由服务端决定的修订数据；API 未提供时为 <see langword="null"/>。<br/>
    /// Gets server-defined revision data, or <see langword="null"/> when omitted by the API.
    /// </summary>
    [JsonPropertyName("data")]
    public JsonElement? Data { get; init; }
}
