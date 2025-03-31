using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    using StudentAggregate = Domain.Aggregates.Student;
    public class SignUpHandler(
        IStudentApplicationService applicationService,
        ILogger logger) : ICommandHandler<SignUpCommand, IdentifierResponse>
    {

        public async Task<Result<IdentifierResponse>> Handle(SignUpCommand cmd, CancellationToken cancellationToken)
        {
            var student = StudentAggregate.Create(cmd.Name, cmd.Email, cmd.Password, cmd.Phone, cmd.DateOfBirth);

            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success<IdentifierResponse>(new (student.Id));
        }
    }
}
