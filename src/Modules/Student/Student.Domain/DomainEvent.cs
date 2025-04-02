using Core.Domain.Primitives;

namespace Student.Domain
{
    public static class DomainEvent
    {
        public record StudentCreated(
            Guid StudentId,
            string Email,
            string Name,
            string Phone,
            string Status,
            DateTimeOffset DateOfBirth,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;

        public record StudentDeleted(
            Guid StudentId,
            ulong Version) : Message, IDomainEvent;

        public record StudentDefaultStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record StudentBlockedStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record StudentActiveStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;
        public record FriendAdded(
            Guid StudentId,
            Guid Friend,
            ulong Version) : Message, IDomainEvent;

        public record XpAdded(
            Guid StudentId,
            int XpAmount,
            bool LeveledUp,
            ulong Version) : Message, IDomainEvent;
        public record StreakAdded(
            Guid StudentId,
            ulong Version) : Message, IDomainEvent;
        public record SubjectProgressCreated(
            Guid Id,
            Guid SubjectId,
            Guid StudentId,
            ulong Version) : Message, IDomainEvent;
        public record SubjectProgressFinishedStatus(
            Guid SubjectId,
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;
        public record LessonProgressCreated(
            Guid Id,
            Guid LessonId,
            Guid SubjectId,
            Guid StudentId,
            ulong Version) : Message, IDomainEvent;
        public record LessonProgressFinishedStatus(
            Guid LessonId,
            Guid SubjectId,
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;
        public record AttemptCreated(
            Guid AttemptId,
            Guid StudentId,
            Guid SubjectId,
            Guid LessonId,
            ulong Version) : Message, IDomainEvent;

        public record AttemptAnswered(
            Guid AttemptId,
            Guid StudentId,
            Guid SubjectId,
            Guid LessonId,
            Guid QuestionId,
            Guid AnswerId,
            bool CorrectAnswer,
            ulong Version) : Message, IDomainEvent;

        public record AttemptInProgressStatus(
            Guid AttemptId,
            Guid LessonId,
            Guid SubjectId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record AttemptFinishedStatus(
            Guid AttemptId,
            Guid LessonId,
            Guid SubjectId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record FriendRequestCreatedStatus(
            Guid RequestId,
            Guid StudentId,
            Guid FriendId,
            ulong Version) : Message, IDomainEvent;

        public record FriendRequestAcceptedStatus(
            Guid RequestId,
            Guid StudentId,
            Guid FriendId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record FriendRequestRejectedStatus(
            Guid RequestId,
            string Status,
            ulong Version) : Message, IDomainEvent;
    }
}
