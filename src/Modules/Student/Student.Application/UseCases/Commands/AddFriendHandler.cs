using Core.Application.Messaging;
using Core.Shared.Results;
using Serilog;
using Student.Application.Services;
using Student.Shared.Commands;

namespace Student.Application.UseCases.Commands
{
    public class AddFriendHandler(
        IStudentApplicationService applicationService,
        ILogger logger) : ICommandHandler<AddFriendCommand>
    {

        public async Task<Result> Handle(AddFriendCommand cmd, CancellationToken cancellationToken)
        {
            var studentResult = await applicationService.LoadAggregateAsync<Domain.Aggregates.Student>(cmd.StudentId, cancellationToken);

            if (studentResult.IsFailure)
                return Result.Failure(studentResult.Error);

            var student = studentResult.Value;

            if(!student.Friends.Any(x => x.StudentId == cmd.FriendId))
                student.AddFriend(cmd.FriendId);

            return Result.Success();
        }
    }
}
