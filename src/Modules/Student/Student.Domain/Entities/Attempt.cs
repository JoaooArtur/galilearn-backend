using Core.Domain.Primitives;
using Student.Domain.Enumerations;

namespace Student.Domain.Entities
{
    public class Attempt : Entity
    {
        public Guid LessonId { get; private set; }
        public int CorrectAnswers { get; private set; }
        public int QuestionsAnswered { get; private set; }
        public AttemptStatus Status { get; private set; }

        public Attempt(
            Guid id,
            Guid lessonId,
            int correctAnswers,
            int questionsAnswered,
            AttemptStatus status,
            DateTimeOffset createdAt)
        {
            Id = id;
            LessonId = lessonId;
            CorrectAnswers = correctAnswers;
            QuestionsAnswered = questionsAnswered;
            Status = status;
            CreatedAt = createdAt;
        }

        public static Attempt Create(Guid id, Guid lessonId)
            => new(id, lessonId, 0, 0, AttemptStatus.Pending, DateTimeOffset.Now);

        public void AnswerQuestion(bool correctAnswer)
        {
            if(correctAnswer)
                CorrectAnswers++;

            QuestionsAnswered++;

            if(QuestionsAnswered == 5)
                Status = AttemptStatus.Finished;
        }
        public void ChangeStatus(AttemptStatus status) => Status = status;

        public static Attempt Undefined
            => new(Guid.Empty, Guid.Empty,0 , 0, AttemptStatus.Pending, DateTimeOffset.Now);
    }
}
