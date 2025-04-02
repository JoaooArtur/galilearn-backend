using Core.Application.Messaging;

namespace Student.Shared.Commands
{
    public sealed record AcceptFriendRequestCommand(Guid StudentId, Guid RequestId) : ICommand;
}
