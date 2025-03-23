using Core.Domain.Primitives;

namespace Student.Domain
{
    public static class DomainEvent
    {
        public record StudentCreated(
            Guid StudentId,
            string Email,
            string Name,
            string Phone,
            string Status,
            DateTimeOffset DateOfBirth,
            DateTimeOffset CreatedAt,
            ulong Version) : Message, IDomainEvent;

        public record StudentDeleted(
            Guid StudentId,
            ulong Version) : Message, IDomainEvent;

        public record StudentDefaultStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record StudentBlockedStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;

        public record StudentActiveStatus(
            Guid StudentId,
            string Status,
            ulong Version) : Message, IDomainEvent;
        public record FriendAdded(
            Guid StudentId,
            Guid Friend,
            ulong Version) : Message, IDomainEvent;
    }
}
