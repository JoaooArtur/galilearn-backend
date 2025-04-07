using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Extensions;
using Student.Application.Services;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    using StudentAggregate = Domain.Aggregates.Student;
    public class SignInHandler(
        IStudentProjection<Projection.Student> studentProjectionGateway,
        IStudentApplicationService applicationService,
        TokenService tokenService,
        ILogger logger) : ICommandHandler<SignInCommand, SignInResponse>
    {

        public async Task<Result<SignInResponse>> Handle(SignInCommand cmd, CancellationToken cancellationToken)
        {
            var studentProjection = await studentProjectionGateway.FindAsync(x => x.Email == cmd.Email, cancellationToken);

            if (studentProjection is null)
                return Result.Failure<SignInResponse>(new Core.Shared.Errors.Error("SignIn", "Verifique as informações de login."));

            var studentResult = await applicationService.LoadAggregateAsync<StudentAggregate>(studentProjection.Id, cancellationToken);

            if(studentResult.IsFailure)
                return Result.Failure<SignInResponse>(new Core.Shared.Errors.Error("SignIn", "Verifique as informações de login."));

            var student = studentResult.Value;

            if(student.Password != cmd.Password.HashMD5())
                return Result.Failure<SignInResponse>(new Core.Shared.Errors.Error("SignIn", "Verifique as informações de login."));

            return Result.Success<SignInResponse>(new(tokenService.GenerateToken(student.Name, student.Id, student.Email, "student")));
        }
    }
}
