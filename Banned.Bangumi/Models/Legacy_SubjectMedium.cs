using System.Text.Json.Serialization;
using Banned.Bangumi.Models.Subjects;

namespace Banned.Bangumi.Models;

public partial record LegacySubjectMedium : LegacySubjectSummary
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [JsonPropertyName("crt")]
    public ICollection<Crt>? Crt { get; init; }
    /// <summary>
    /// 制作人员信息
    /// </summary>
    [JsonPropertyName("staff")]
    public ICollection<Staff>? Staff { get; init; }
}
