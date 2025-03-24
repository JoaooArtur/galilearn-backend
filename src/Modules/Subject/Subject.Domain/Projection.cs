using Core.Domain.Primitives;
using Subject.Domain.Enumerations;

namespace Subject.Domain
{
    public static class Projection
    {
        public record Subject(
            Guid Id,
            string Name,
            string Description,
            int Index,
            DateTimeOffset CreatedAt) : IProjection
        { }

        public record Lesson(
            Guid Id,
            Guid SubjectId,
            string Title,
            string Content,
            int Index,
            DateTimeOffset CreatedAt) : IProjection
        { }

        public record Question(
            Guid Id,
            Guid LessonId,
            string Text,
            string Level,
            List<Dto.AnswerOption> Answers,
            DateTimeOffset CreatedAt) : IProjection
        { }
    }
}
