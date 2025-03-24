
using Core.Application.Messaging;
using Subject.Shared.Response;

namespace Subject.Shared.Commands
{
    public sealed record CreateLessonCommand(Guid SubjectId, string Title, string Content, int Index) : ICommand<IdentifierResponse>;
}
