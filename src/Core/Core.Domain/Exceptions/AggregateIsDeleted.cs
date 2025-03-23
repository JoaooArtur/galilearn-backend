namespace Core.Domain.Exceptions
{
    public class AggregateIsDeleted(Guid aggregateId) : Exception($"Aggregate with AggregateId '{aggregateId}' deleted.") { }
}
