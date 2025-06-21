using Core.Application.Messaging;
using Core.Shared.Results;
using MathNet.Numerics.Distributions;
using Serilog;
using Student.Application.Services;
using Student.Domain.Enumerations;
using Student.Shared.Commands;

namespace Student.Application.UseCases.Commands
{
    public class AcceptFriendRequestHandler(
        IStudentApplicationService applicationService) : ICommandHandler<AcceptFriendRequestCommand>
    {

        public async Task<Result> Handle(AcceptFriendRequestCommand cmd, CancellationToken cancellationToken)
        {

            var studentResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Student>(cmd.StudentId, cancellationToken);

            if (studentResult.IsFailure)
                return Result.Failure(studentResult.Error);

            var student = studentResult.Value;

            if (student.Requests.Any(x => x.Id == cmd.RequestId && x.Status == FriendRequestStatus.Accepted))
                return Result.Failure(new Core.Shared.Errors.Error("FriendAlreadyAdded", "Vocês já são amigos!"));

            var request = student.Requests.FirstOrDefault(x => x.Id == cmd.RequestId);

            student.AcceptFriendRequest(cmd.RequestId, request.FriendId);

            await applicationService.AppendEventsAsync(student, cancellationToken);

            return Result.Success();
        }
    }
}
