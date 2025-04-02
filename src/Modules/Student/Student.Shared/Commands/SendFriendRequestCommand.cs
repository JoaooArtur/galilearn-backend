using Core.Application.Messaging;

namespace Student.Shared.Commands
{
    public sealed record SendFriendRequestCommand(Guid StudentId, Guid FriendId) : ICommand;
}
