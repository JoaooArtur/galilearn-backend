using Core.Domain.Primitives;
using Student.Domain.Enumerations;

namespace Student.Domain.Entities
{
    public class FriendRequest : Entity
    {
        public Guid FriendId { get; private set; }
        public FriendRequestStatus Status { get; private set; }

        public FriendRequest(
            Guid id,
            Guid friendId,
            FriendRequestStatus status,
            DateTimeOffset createdAt)
        {
            Id = id;
            FriendId = friendId;
            Status = status;
            CreatedAt = createdAt;
        }

        public static FriendRequest Create(Guid id, Guid friendId)
            => new(id, friendId, FriendRequestStatus.Pending, DateTimeOffset.Now);

        public void ChangeStatus(FriendRequestStatus status) => Status = status;

        public static FriendRequest Undefined
            => new(Guid.Empty, Guid.Empty, FriendRequestStatus.Pending, DateTimeOffset.Now);
    }
}
