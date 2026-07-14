namespace Banned.Bangumi.Models.Common;

/// <summary>
/// 指定图片尺寸；不同资源支持的尺寸子集可能不同。<br/>
/// Specifies an image size; supported subsets may differ between resources.
/// </summary>
public enum ImageSize
{
    /// <summary>小尺寸。 / Small size.</summary>
    Small,

    /// <summary>
    /// 网格尺寸；用户头像接口不支持。<br/>
    /// Grid size; not supported by the user avatar endpoint.
    /// </summary>
    Grid,

    /// <summary>大尺寸。 / Large size.</summary>
    Large,

    /// <summary>中等尺寸。 / Medium size.</summary>
    Medium,

    /// <summary>
    /// 常用尺寸；仅条目图片接口支持。<br/>
    /// Common size; supported only by the subject image endpoint.
    /// </summary>
    Common,
}
