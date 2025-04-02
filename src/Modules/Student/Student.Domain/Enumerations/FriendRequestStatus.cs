using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Student.Domain.Enumerations
{
    [method: JsonConstructor]
    public class FriendRequestStatus(string name, int value) : SmartEnum<FriendRequestStatus>(name, value)
    {
        public static readonly FriendRequestStatus Pending = new PendingStatus();
        public static readonly FriendRequestStatus Accepted = new AcceptedStatus();
        public static readonly FriendRequestStatus Rejected = new RejectedStatus();

        public static implicit operator FriendRequestStatus(string name)
            => FromName(name);

        public static implicit operator FriendRequestStatus(int value)
            => FromValue(value);

        public static implicit operator string(FriendRequestStatus status)
            => status.Name;

        public static implicit operator int(FriendRequestStatus status)
            => status.Value;

        public class PendingStatus() : FriendRequestStatus(nameof(Pending), 0) { }
        public class AcceptedStatus() : FriendRequestStatus(nameof(Accepted), 1) { }
        public class RejectedStatus() : FriendRequestStatus(nameof(Rejected), 2) { }
    }
}
