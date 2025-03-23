namespace Core.Domain.Primitives;

public abstract class Entity : IEntity
{
    public Guid Id { get; protected set; } = new();
    public bool IsDeleted { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;

    public static bool operator ==(Entity left, Entity right)
        => left.Id.Equals(right.Id);

    public static bool operator !=(Entity left, Entity right)
        => left.Id.Equals(right.Id) is false;

    public override bool Equals(object? obj)
        => obj is Entity entity && Id.Equals(entity.Id);

    public override int GetHashCode()
        => HashCode.Combine(Id);
}
