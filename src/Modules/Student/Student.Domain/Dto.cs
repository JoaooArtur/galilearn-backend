namespace Student.Domain
{
    public static class Dto
    {
        public record Friend(
            Guid StudentId);

        public record Email(
            Guid Id,
            string Address,
            bool IsConfirmed,
            DateTimeOffset CreatedAt);
    }
}
