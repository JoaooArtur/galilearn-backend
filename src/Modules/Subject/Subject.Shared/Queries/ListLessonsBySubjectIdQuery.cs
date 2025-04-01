using Core.Application.Messaging;
using Subject.Shared.Response;

namespace Subject.Shared.Queries
{
    public sealed record ListLessonsBySubjectIdQuery(Guid SubjectId) : IQuery<List<PagedLessonBySubjectIdResponse>>;
}
