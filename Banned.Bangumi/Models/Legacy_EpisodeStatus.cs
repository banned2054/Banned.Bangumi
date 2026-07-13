namespace Banned.Bangumi;

public enum LegacyEpisodeStatus
{
    [System.Runtime.Serialization.EnumMember(Value = @"Air")]
    Air = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"Today")]
    Today = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"NA")]
    NA = 2,
}
