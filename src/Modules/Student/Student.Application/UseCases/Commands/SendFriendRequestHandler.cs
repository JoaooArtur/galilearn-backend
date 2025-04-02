using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;

namespace Student.Application.UseCases.Commands
{
    public class SendFriendRequestHandler(
        IStudentApplicationService applicationService,
        ILogger logger) : ICommandHandler<SendFriendRequestCommand>
    {

        public async Task<Result> Handle(SendFriendRequestCommand cmd, CancellationToken cancellationToken)
        {

            var friendResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Student>(cmd.FriendId, cancellationToken);

            if (friendResult.IsFailure)
                return Result.Failure(friendResult.Error);

            var friend = friendResult.Value;

            if (friend.Friends.Any(x => x.StudentId == cmd.StudentId))
                return Result.Failure(new Core.Shared.Errors.Error("FriendAlreadyAdded", "Vocês já são amigos!"));

            friend.CreateFriendRequest(cmd.StudentId);

            await applicationService.AppendEventsAsync(friend, cancellationToken);

            return Result.Success();
        }
    }
}
