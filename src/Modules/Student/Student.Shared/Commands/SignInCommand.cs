using Core.Application.Messaging;
using Student.Shared.Response;

namespace Student.Shared.Commands
{
    public sealed record SignInCommand(string Email, string Password) : ICommand<SignInResponse>;
}
