using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    public class SignUpHandler(
        IStudentApplicationService applicationService,
        ILogger logger) : ICommandHandler<SignUpCommand, IdentifierResponse>
    {

        public async Task<Result<IdentifierResponse>> Handle(SignUpCommand cmd, CancellationToken cancellationToken)
        {
            return Result.Success<IdentifierResponse>(new (Guid.NewGuid()));
        }
    }
}
