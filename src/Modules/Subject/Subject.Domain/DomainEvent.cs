using Core.Domain.Primitives;
using Subject.Domain.Enumerations;

namespace Subject.Domain
{
    public static class DomainEvent
    {
        public record SubjectCreated(
            Guid SubjectId,
            string Name,
            string Description,
            int Index,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;

        public record SubjectDeleted(
            Guid SubjectId,
            ulong Version) : Message, IDomainEvent;

        public record LessonAdded(
            Guid SubjectId,
            Guid LessonId,
            string Title,
            string Content,
            int Index,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;

        public record QuestionAdded(
            Guid QuestionId,
            Guid LessonId,
            string Text,
            QuestionLevel Level,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;
        public record AnswerOptionAdded(
            Guid LessonId,
            Guid QuestionId,
            Guid AnswerOptionId,
            string Text,
            bool IsRightAnswer,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;
    }
}
