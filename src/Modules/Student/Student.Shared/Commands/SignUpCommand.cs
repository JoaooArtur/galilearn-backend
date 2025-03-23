using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Commands
{
    public sealed record SignUpCommand() : ICommand<IdentifierResponse>;
}
