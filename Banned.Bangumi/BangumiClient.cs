using Banned.Bangumi.Services;
using Banned.Bangumi.Services.Internal;

namespace Banned.Bangumi;

/// <summary>
/// 提供 Bangumi API 的统一入口。<br/>
/// Provides the main entry point for the Bangumi API.
/// </summary>
public sealed class BangumiClient : IDisposable
{
    private readonly BangumiHttpService _httpService;

    /// <summary>
    /// 初始化 <see cref="BangumiClient"/> 类的新实例。<br/>
    /// Initializes a new instance of the <see cref="BangumiClient"/> class.
    /// </summary>
    /// <param name="options">客户端配置。 / The client configuration.</param>
    public BangumiClient(BangumiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _httpService = new BangumiHttpService(options);
        Calendar     = new CalendarService(_httpService);
        Subjects     = new SubjectService(_httpService);
        Episodes     = new EpisodeService(_httpService);
        Characters   = new CharacterService(_httpService);
        Persons      = new PersonService(_httpService);
        Users        = new UserService(_httpService);
        Collections  = new CollectionService(_httpService);
        Revisions    = new RevisionService(_httpService);
        Indices      = new IndexService(_httpService);
    }

    /// <summary>
    /// 获取每日放送操作。<br/>
    /// Gets calendar operations.
    /// </summary>
    public CalendarService Calendar { get; }

    /// <summary>
    /// 获取条目操作。<br/>
    /// Gets subject operations.
    /// </summary>
    public SubjectService Subjects { get; }

    /// <summary>
    /// 获取章节操作。<br/>
    /// Gets episode operations.
    /// </summary>
    public EpisodeService Episodes { get; }

    /// <summary>
    /// 获取角色操作。<br/>
    /// Gets character operations.
    /// </summary>
    public CharacterService Characters { get; }

    /// <summary>
    /// 获取人物操作。<br/>
    /// Gets person operations.
    /// </summary>
    public PersonService Persons { get; }

    /// <summary>
    /// 获取用户操作。<br/>
    /// Gets user operations.
    /// </summary>
    public UserService Users { get; }

    /// <summary>
    /// 获取收藏操作。<br/>
    /// Gets collection operations.
    /// </summary>
    public CollectionService Collections { get; }

    /// <summary>
    /// 获取修订操作。<br/>
    /// Gets revision operations.
    /// </summary>
    public RevisionService Revisions { get; }

    /// <summary>
    /// 获取目录操作。<br/>
    /// Gets index operations.
    /// </summary>
    public IndexService Indices { get; }

    /// <inheritdoc />
    public void Dispose() => _httpService.Dispose();
}
