namespace Core.Domain.Primitives;

public interface IMessage
{
    DateTimeOffset Timestamp { get; }
}
