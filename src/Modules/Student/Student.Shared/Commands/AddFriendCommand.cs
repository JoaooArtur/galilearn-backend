using Core.Application.Messaging;

namespace Student.Shared.Commands
{
    public sealed record AddFriendCommand(Guid StudentId, Guid FriendId) : ICommand;
}
