using Banned.Bangumi.Models.Calendar;
using Banned.Bangumi.Models.Characters;
using Banned.Bangumi.Models.Collections;
using Banned.Bangumi.Models.Common;
using Banned.Bangumi.Models.Episodes;
using Banned.Bangumi.Models.Indices;
using Banned.Bangumi.Models.Persons;
using Banned.Bangumi.Models.Revisions;
using Banned.Bangumi.Models.Subjects;
using Banned.Bangumi.Models.Users;
using System.Text.Json.Serialization;

namespace Banned.Bangumi.Serialization;

[JsonSerializable(typeof(SubjectSearchRequest))]
[JsonSerializable(typeof(CharacterSearchRequest))]
[JsonSerializable(typeof(PersonSearchRequest))]
[JsonSerializable(typeof(SubjectCollectionUpdateRequest))]
[JsonSerializable(typeof(EpisodeCollectionBatchUpdateRequest))]
[JsonSerializable(typeof(EpisodeCollectionUpdateRequest))]
[JsonSerializable(typeof(IndexUpdateRequest))]
[JsonSerializable(typeof(IndexSubjectAddRequest))]
[JsonSerializable(typeof(IndexSubjectUpdateRequest))]
[JsonSerializable(typeof(IReadOnlyList<CalendarDay>))]
[JsonSerializable(typeof(PagedResult<Subject>))]
[JsonSerializable(typeof(Subject))]
[JsonSerializable(typeof(IReadOnlyList<RelatedPerson>))]
[JsonSerializable(typeof(IReadOnlyList<RelatedCharacter>))]
[JsonSerializable(typeof(IReadOnlyList<SubjectRelation>))]
[JsonSerializable(typeof(PagedResult<Episode>))]
[JsonSerializable(typeof(EpisodeDetail))]
[JsonSerializable(typeof(Character))]
[JsonSerializable(typeof(IReadOnlyList<RelatedSubject>))]
[JsonSerializable(typeof(IReadOnlyList<CharacterRelatedPerson>))]
[JsonSerializable(typeof(PagedResult<Character>))]
[JsonSerializable(typeof(PersonDetail))]
[JsonSerializable(typeof(IReadOnlyList<PersonRelatedCharacter>))]
[JsonSerializable(typeof(PagedResult<Person>))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(CurrentUser))]
[JsonSerializable(typeof(PagedResult<UserSubjectCollection>))]
[JsonSerializable(typeof(UserSubjectCollection))]
[JsonSerializable(typeof(PagedResult<UserEpisodeCollection>))]
[JsonSerializable(typeof(UserEpisodeCollection))]
[JsonSerializable(typeof(PagedResult<UserCharacterCollection>))]
[JsonSerializable(typeof(UserCharacterCollection))]
[JsonSerializable(typeof(PagedResult<UserPersonCollection>))]
[JsonSerializable(typeof(UserPersonCollection))]
[JsonSerializable(typeof(PagedResult<Revision>))]
[JsonSerializable(typeof(PersonRevision))]
[JsonSerializable(typeof(CharacterRevision))]
[JsonSerializable(typeof(SubjectRevision))]
[JsonSerializable(typeof(EpisodeRevision))]
[JsonSerializable(typeof(BangumiIndex))]
[JsonSerializable(typeof(PagedResult<IndexSubject>))]
[JsonSerializable(typeof(ErrorDetail))]
internal sealed partial class BangumiJsonSerializerContext : JsonSerializerContext;
