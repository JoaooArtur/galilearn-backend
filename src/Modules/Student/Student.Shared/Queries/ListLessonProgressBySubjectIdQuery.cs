using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Queries
{
    public sealed record ListLessonProgressBySubjectIdQuery(Guid SubjectId, Guid StudentId) : IQuery<List<LessonsProgressBySubjectIdResponse>>;
}
