namespace Core.Domain.Primitives;

public interface IEntity
{
    Guid Id { get; }
    bool IsDeleted { get; }
    DateTimeOffset CreatedAt { get; }
}
