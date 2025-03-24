using Core.Application.Messaging;
using Subject.Domain.Enumerations;
using Subject.Shared.Response;

namespace Subject.Shared.Commands
{
    public sealed record CreateQuestionCommand(Guid SubjectId, Guid LessonId, string Text, QuestionLevel Level) : ICommand<IdentifierResponse>;

}
