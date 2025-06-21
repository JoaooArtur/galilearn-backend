using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;

namespace Student.Application.UseCases.Commands
{
    public class RejectFriendRequestHandler(
        IStudentApplicationService applicationService) : ICommandHandler<RejectFriendRequestCommand>
    {

        public async Task<Result> Handle(RejectFriendRequestCommand cmd, CancellationToken cancellationToken)
        {

            var studentResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Student>(cmd.StudentId, cancellationToken);

            if (studentResult.IsFailure)
                return Result.Failure(studentResult.Error);

            var student = studentResult.Value;

            student.RejectFriendRequest(cmd.RequestId);

            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success();
        }
    }
}
