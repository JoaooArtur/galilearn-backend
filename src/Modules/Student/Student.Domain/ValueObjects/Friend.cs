namespace Student.Domain.ValueObjects
{
    public record Friend(Guid StudentId)
    {
        public static Friend Create(Dto.Friend dto)
            => new(dto.StudentId);

        public static implicit operator Dto.Friend(Friend friend)
            => new(friend.StudentId);

        public static bool operator ==(Friend friend, Dto.Friend dto)
            => dto == (Dto.Friend)friend;

        public static bool operator !=(Friend friend, Dto.Friend dto)
            => dto != (Dto.Friend)friend;

        public static Friend Undefined
            => new(Guid.Empty);

        public static implicit operator Friend(Dto.Friend friend)
            => new(friend.StudentId);
    }
}
