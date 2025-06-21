namespace Core.Domain.Primitives;

public abstract class Entity : IEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;

    public override bool Equals(object? obj)
        => obj is Entity entity && Id.Equals(entity.Id);

    public override int GetHashCode()
        => HashCode.Combine(Id);
}
