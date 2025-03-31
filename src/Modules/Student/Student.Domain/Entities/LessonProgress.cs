

using Core.Domain.Primitives;
using Student.Domain.Enumerations;

namespace Student.Domain.Entities
{
    public class LessonProgress : Entity
    {
        public Guid LessonId { get; private set; }
        public LessonStatus Status { get; private set; }
        public List<Attempt> Attempts { get; private set; } = [];

        public LessonProgress(
            Guid id,
            Guid lessonId,
            LessonStatus status,
            DateTimeOffset createdAt)
        {
            Id = id;
            LessonId = lessonId;
            Status = status;
            CreatedAt = createdAt;
        }

        public static LessonProgress Create(Guid id, Guid lessonId)
            => new(id, lessonId, LessonStatus.Pending, DateTimeOffset.Now);

        public Guid CreateAttempt(Guid lessonId)
        {
            var attemptId = Guid.NewGuid();
            Attempts.Add(Attempt.Create(attemptId, lessonId));
            return attemptId;
        }
        public void ChangeStatus(LessonStatus status) => Status = status;

        public static LessonProgress Undefined
            => new(Guid.Empty, Guid.Empty, LessonStatus.Pending, DateTimeOffset.Now);
    }
}
