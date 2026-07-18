using Banned.Bangumi.Serialization;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 指定搜索结果的 NSFW 限制方式。<br/>
/// Specifies how search results are restricted by NSFW status.
/// </summary>
[JsonConverter(typeof(NsfwFilterModeJsonConverter))]
public enum NsfwFilterMode
{
    /// <summary>
    /// 排除 NSFW 结果，仅返回非 NSFW 结果。<br/>
    /// Excludes NSFW results and returns only non-NSFW results.
    /// </summary>
    Exclude,

    /// <summary>
    /// 仅返回 NSFW 结果。<br/>
    /// Returns only NSFW results.
    /// </summary>
    Only,
}
