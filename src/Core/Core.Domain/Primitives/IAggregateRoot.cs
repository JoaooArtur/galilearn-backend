namespace Core.Domain.Primitives;

public interface IAggregateRoot : IEntity
{
    ulong Version { get; }
    void LoadFromStream(List<IDomainEvent> events);
    bool TryDequeueEvent(out IDomainEvent @event);
}
