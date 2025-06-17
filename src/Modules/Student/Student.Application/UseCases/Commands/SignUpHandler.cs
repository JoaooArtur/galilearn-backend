using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Extensions;
using Student.Application.Services;
using Student.Domain;
using Student.Domain.Enumerations;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    using StudentAggregate = Domain.Aggregates.Student;
    public class SignUpHandler(
        IStudentApplicationService applicationService,
        IStudentProjection<Projection.Student> studentProjection,
        ILogger logger) : ICommandHandler<SignUpCommand, IdentifierResponse>
    {

        public async Task<Result<IdentifierResponse>> Handle(SignUpCommand cmd, CancellationToken cancellationToken)
        {
            var existingStudent = await studentProjection.FindAsync(x => x.Email == cmd.Email && x.Status == StudentStatus.Active, cancellationToken);
            if(existingStudent is not null)
                return Result.Failure<IdentifierResponse>(new Core.Shared.Errors.Error("EmailAlreadyExists", "O Email informado já esta sendo utilizado."));

            var student = StudentAggregate.Create(cmd.Name, cmd.Email, cmd.Password.HashMD5(), cmd.Phone, cmd.DateOfBirth);

            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success<IdentifierResponse>(new (student.Id));
        }
    }
}
