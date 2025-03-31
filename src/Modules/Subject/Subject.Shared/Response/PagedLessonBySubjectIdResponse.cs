
namespace Subject.Shared.Response
{
    public sealed record PagedLessonBySubjectIdResponse(
            Guid Id,
            Guid SubjectId,
            string Title,
            string Content,
            int Index,
            DateTimeOffset CreatedAt);
}
