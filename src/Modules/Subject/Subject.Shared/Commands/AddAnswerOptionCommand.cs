using Core.Application.Messaging;
using Subject.Shared.Response;

namespace Subject.Shared.Commands
{
    public sealed record AddAnswerOptionCommand(Guid SubjectId, Guid LessonId, Guid QuestionId, string Text, bool IsRightAnswer = false) : ICommand<IdentifierResponse>;
}
