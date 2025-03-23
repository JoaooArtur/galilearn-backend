using Core.Domain.Primitives;

namespace Student.Domain
{
    public static class Projection
    {
        public record Student(
            Guid Id,
            string Name,
            string Phone,
            string Email,
            string Status,
            DateTimeOffset DateOfBirth,
            DateTimeOffset CreatedAt) : IProjection
        { }
    }
}
