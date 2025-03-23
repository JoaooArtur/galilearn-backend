namespace Core.Domain.Primitives;

public abstract record Message
{
    public DateTimeOffset Timestamp { get; private init; } = DateTimeOffset.Now;
}
