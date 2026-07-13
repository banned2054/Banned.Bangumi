using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models;

public partial record Response : User
{
    /// <summary>
    /// 用户绑定的邮箱地址
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("reg_time")]
    public DateTimeOffset? RegTime { get; init; }

    [JsonPropertyName("time_offset")]
    public int? TimeOffset { get; init; }
}