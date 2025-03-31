using Core.Application.Messaging;
using Core.Domain.Primitives;
using Subject.Shared.Response;

namespace Subject.Shared.Queries
{
    public sealed record PagedLessonsBySubjectIdQuery(Guid SubjectId, Paging Paging) : IQuery<IPagedResult<PagedLessonBySubjectIdResponse>>;
}
