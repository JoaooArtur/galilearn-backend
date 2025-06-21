namespace Core.Domain.Exceptions
{
    public class AggregateIsDeletedException(Guid aggregateId) : Exception($"Aggregate with AggregateId '{aggregateId}' deleted.") { }
}
