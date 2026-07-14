using Banned.Bangumi.Models.Common;
using System.Collections.ObjectModel;

namespace Banned.Bangumi.Models;

/// <summary>
/// 表示旧版条目响应中的标签集合。<br/>
/// Represents the tag collection in a legacy subject response.
/// </summary>
public partial class SubjectTags : Collection<Tag>
{
}
