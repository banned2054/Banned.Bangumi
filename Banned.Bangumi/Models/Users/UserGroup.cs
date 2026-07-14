namespace Banned.Bangumi.Models.Users;

/// <summary>
/// 指定用户组。<br/>
/// Specifies a user group.
/// </summary>
public enum UserGroup
{
    /// <summary>管理员。 / Administrator.</summary>
    Admin = 1,

    /// <summary>Bangumi 管理猿。 / Bangumi administrator.</summary>
    BangumiAdmin = 2,

    /// <summary>天窗管理猿。 / Doujin administrator.</summary>
    DoujinAdmin = 3,

    /// <summary>禁言用户。 / Muted user.</summary>
    MutedUser = 4,

    /// <summary>禁止访问用户。 / Blocked user.</summary>
    BlockedUser = 5,

    /// <summary>人物管理猿。 / Person administrator.</summary>
    PersonAdmin = 8,

    /// <summary>维基条目管理猿。 / Wiki administrator.</summary>
    WikiAdmin = 9,

    /// <summary>普通用户。 / Regular user.</summary>
    User = 10,

    /// <summary>维基人。 / Wiki contributor.</summary>
    WikiUser = 11,
}
