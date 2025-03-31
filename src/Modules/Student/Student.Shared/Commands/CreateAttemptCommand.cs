using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Commands
{
    public sealed record CreateAttemptCommand(Guid StudentId, Guid SubjectId, Guid LessonId) : ICommand<IdentifierResponse>;
}
