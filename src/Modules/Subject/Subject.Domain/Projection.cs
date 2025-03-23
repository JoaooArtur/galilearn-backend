using Core.Domain.Primitives;

namespace Subject.Domain
{
    public static class Projection
    {
        public record Subject(
            Guid Id,
            string Name,
            string Description,
            int Index,
            DateTimeOffset CreatedAt) : IProjection
        { }
    }
}
