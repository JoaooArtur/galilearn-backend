using Core.Domain.Primitives;

namespace Student.Domain
{
    public static class Projection
    {
        public record Student(
            Guid Id,
            string Name,
            string Phone,
            string Email,
            string Status,
            int Level,
            int Xp,
            int NextLevelXPNeeded,
            int DaysStreak,
            List<Guid> Friends,
            DateTimeOffset DateOfBirth,
            DateTimeOffset? LastLessonAnswered,
            DateTimeOffset CreatedAt) : IProjection
        { }
        public record FriendRequests(
            Guid Id,
            Guid StudentId,
            Guid FriendId,
            string Status,
            DateTimeOffset CreatedAt) : IProjection
        { }

        public record Attempt(
            Guid Id,
            Guid StudentId,
            Guid LessonId,
            int CorrectAnswers,
            int QuestionsAnswered,
            string Status,
            DateTimeOffset CreatedAt) : IProjection
        { }
        public record SubjectProgress(
            Guid Id,
            Guid SubjectId,
            Guid StudentId,
            string Status,
            DateTimeOffset CreatedAt) : IProjection
        { }
        public record LessonProgress(
            Guid Id,
            Guid SubjectId,
            Guid LessonId,
            Guid StudentId,
            string Status,
            DateTimeOffset CreatedAt) : IProjection
        { }
    }
}
