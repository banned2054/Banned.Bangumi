using System.Runtime.Serialization;

namespace Banned.Bangumi.Models.Enums;

public enum BodySort
{
    [EnumMember(Value = "match")]
    Match = 0,

    [EnumMember(Value = "heat")]
    Heat = 1,

    [EnumMember(Value = "rank")]
    Rank = 2,

    [EnumMember(Value = "score")]
    Score = 3,
}