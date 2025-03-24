using Core.Application.Messaging;
using Subject.Shared.Response;

namespace Subject.Shared.Commands
{
    public sealed record CreateSubjectCommand(string Name, string Description, int Index) : ICommand<IdentifierResponse>;
}
