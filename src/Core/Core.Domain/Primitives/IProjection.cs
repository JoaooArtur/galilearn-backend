namespace Core.Domain.Primitives
{
    public interface IProjection
    {
        Guid Id { get; }

        DateTimeOffset CreatedAt { get; }
    }
}
