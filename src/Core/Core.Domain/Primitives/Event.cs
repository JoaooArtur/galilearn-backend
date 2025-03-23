namespace Core.Domain.Primitives;

public abstract record Event : Message, IEvent;
