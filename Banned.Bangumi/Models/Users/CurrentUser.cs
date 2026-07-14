using System.Text.Json.Serialization;

namespace Banned.Bangumi.Models.Users;

/// <summary>
/// 表示当前访问令牌对应的用户资料。<br/>
/// Represents the profile of the user associated with the current access token.
/// </summary>
public sealed record CurrentUser : User
{
    /// <summary>获取用户绑定的邮箱地址。<br/>Gets the email address bound to the user.</summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    /// <summary>获取用户注册时间。<br/>Gets the user registration time.</summary>
    [JsonPropertyName("reg_time")]
    public DateTimeOffset RegistrationTime { get; init; }

    /// <summary>获取用户设置的时区偏移小时数；未提供时为 <see langword="null"/>。<br/>Gets the user's configured time-zone offset in hours, or <see langword="null"/> when unavailable.</summary>
    [JsonPropertyName("time_offset")]
    public int? TimeOffset { get; init; }
}
