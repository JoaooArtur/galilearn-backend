using Student.Domain.Enumerations;
using Subject.Shared.Response;

namespace Student.Shared.Response
{
    public sealed record LessonsProgressBySubjectIdResponse(
            PagedLessonBySubjectIdResponse Subject,
            LessonStatus Status);
}
