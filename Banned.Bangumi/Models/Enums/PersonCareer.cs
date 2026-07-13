namespace Banned.Bangumi.Models.Enums;

/// <summary>
/// An enumeration.
/// </summary>
public enum PersonCareer
{
    [System.Runtime.Serialization.EnumMember(Value = @"producer")]
    Producer = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"mangaka")]
    Mangaka = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"artist")]
    Artist = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"seiyu")]
    Seiyu = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"writer")]
    Writer = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"illustrator")]
    Illustrator = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"actor")]
    Actor = 6,
}