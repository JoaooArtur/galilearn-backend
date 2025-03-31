using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Commands
{
    public sealed record SignUpCommand(string Name, string Email, string Password, string Phone, DateTimeOffset DateOfBirth) : ICommand<IdentifierResponse>;
}
