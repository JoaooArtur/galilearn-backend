
namespace Subject.Shared.Response
{
    public sealed record LessonResponse(
            Guid Id,
            Guid SubjectId,
            string Title,
            string Content,
            int Index,
            DateTimeOffset CreatedAt);
}
