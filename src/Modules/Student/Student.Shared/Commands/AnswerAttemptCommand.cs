using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Commands
{
    public sealed record AnswerAttemptCommand(Guid StudentId, Guid SubjectId, Guid LessonId, Guid AttemptId, Guid QuestionId, Guid AnswerId) : ICommand<CorrectAnswerResponse>;
}
