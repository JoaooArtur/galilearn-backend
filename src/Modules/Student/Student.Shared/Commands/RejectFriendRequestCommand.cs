
using Core.Application.Messaging;

namespace Student.Shared.Commands
{
    public sealed record RejectFriendRequestCommand(Guid StudentId, Guid RequestId) : ICommand;
}
