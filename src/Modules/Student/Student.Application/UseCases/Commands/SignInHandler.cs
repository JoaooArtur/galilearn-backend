using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Domain;
using Student.Persistence.Projections;
using Student.Shared.Commands;
using Student.Shared.Response;

namespace Student.Application.UseCases.Commands
{
    using StudentAggregate = Domain.Aggregates.Student;
    public class SignInHandler(
        IStudentProjection<Projection.Student> studentProjection,
        ILogger logger) : ICommandHandler<SignInCommand, SignInResponse>
    {

        public async Task<Result<SignInResponse>> Handle(SignInCommand cmd, CancellationToken cancellationToken)
        {
            //var student = StudentAggregate.Create(cmd.Name, cmd.Email, cmd.Password, cmd.Phone, cmd.DateOfBirth);

            //await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success<SignInResponse>(new("eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6InRXQ1JFVjhQYmJxeXdfTS1IUS1BayJ9.eyJjYXJ0YW9zaW1wbGVzL3JvbGVzIjpbInNlbGxlciJdLCJjYXJ0YW9zaW1wbGVzL2lwIjoiMjAwLjIzMy4xNDIuMTQzIiwiaXNzIjoiaHR0cHM6Ly9hdXRoLnN0YWdpbmcuY2FydGFvc2ltcGxlcy5jb20uYnIvIiwic3ViIjoiYXV0aDB8NWI1NGNiM2YtMmQ5Yy00MjM2LWJjNjEtNzVlMzA1M2I3OTQ0IiwiYXVkIjpbImh0dHBzOi8vYXBpLnN0YWdpbmcuY2FydGFvc2ltcGxlcy5jb20uYnIiLCJodHRwczovL2Rldi1sZTZzaGJrbHh2Z2N5ZjY3LnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE3NDM2MjM1NzksImV4cCI6MTc0MzcwOTk3OSwic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCIsImF6cCI6IlZ2dzFONXFVU1ZwNDNFblRFWGJmWElDVVF2amNJTGtjIn0.DmfSMKtQ1nTB2Dzb6VkPA2azEuP3QfjQoc8G1PS7MuIzwMM0mXqVJt6I_xFm9I1I-ep2oj8UYr4c63Z06kcGXy82NHbNw4cJYeg8mCvITEHEtwniVnucwh0e0tBm1DeYraco05D7DqKntYZ9_k46XpoWSGw9Qnn4u_2R-6spnnAz7a_NWmr9MXA02hNzfwOfFgB4sF_Kw8MqyCOQIXBRjx-TYnoHPxEpOjU1gArU9b6Rzra7DiguI1ZkDarSrrXxRXcGk4U-KpyMYzZ_SG152tFmKPWGD1wHW6O5XAosEu-1BKtEXJaxkRXlU2W1ulaebKOAL7IyjM66KPd8oRBUXg"));
        }
    }
}
