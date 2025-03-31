using Core.Domain.Primitives;
using Student.Domain.Enumerations;

namespace Student.Domain.Entities
{
    public class SubjectProgress : Entity
    {
        public Guid SubjectId { get; private set; }
        public SubjectStatus Status { get; private set; }
        public List<LessonProgress> LessonProgresses { get; private set; } = [];

        public SubjectProgress(
            Guid id,
            Guid subjectId,
            SubjectStatus status,
            DateTimeOffset createdAt)
        {
            Id = id;
            SubjectId = subjectId;
            Status = status;
            CreatedAt = createdAt;
        }

        public static SubjectProgress Create(Guid id, Guid subjectId)
            => new(id, subjectId, SubjectStatus.Pending, DateTimeOffset.Now);

        public Guid AddLessonProgress(Guid lessonId)
        {
            var lessonProgressId = Guid.NewGuid();
            LessonProgresses.Add(LessonProgress.Create(lessonProgressId, lessonId));
            return lessonProgressId;
        }

        public void ChangeStatus(SubjectStatus status) => Status = status;

        public static SubjectProgress Undefined
            => new(Guid.Empty, Guid.Empty, SubjectStatus.Pending, DateTimeOffset.Now);
    }
}
