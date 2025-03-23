﻿using Core.Application.EventBus;
using Core.Application.EventStore;
using Core.Domain.EventStore;
using Core.Domain.Primitives;
using Core.Shared.Errors;
using Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public class ApplicationService<TContext>(
        IEventStore<TContext> eventStore,
        IEventBus eventBusGateway,
        IUnitOfWork<TContext> unitOfWork) : IApplicationService<TContext>
        where TContext : DbContext
    {
        public async Task<Result<TAggregate>> LoadAggregateAsync<TAggregate>(Guid id, CancellationToken cancellationToken)
            where TAggregate : class, IAggregateRoot, new()
        {
            var snapshot = await eventStore.GetSnapshotAsync<TAggregate>(id, cancellationToken);
            var events = await eventStore.GetStreamAsync<TAggregate>(id, snapshot?.Version ?? 0, cancellationToken);

            if (snapshot is null && events is { Count: 0 })
                return Result.Failure<TAggregate>(new NotFoundError(new("Aggregate.NotFound", $"Aggregate {typeof(TAggregate).Name} não encontrado.")));

            var aggregate = snapshot?.Aggregate ?? new();
            aggregate.LoadFromStream(events);

            if (aggregate.IsDeleted)
                return Result.Failure<TAggregate>(new ConflictError(new("Aggregate.Deleted", $"Aggregate {typeof(TAggregate).Name} está deletado.")));

            return Result.Success(aggregate);
        }

        public Task AppendEventsAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot
            => unitOfWork.ExecuteAsync(
            operationAsync: async ct =>
            {
                while (aggregate.TryDequeueEvent(out var @event))
                {
                    var storeEvent = StoreEvent<TAggregate>.Create(aggregate, @event);
                    await eventStore.AppendAsync(storeEvent, ct);

                    if (storeEvent.Version % 10 is 0)
                    {
                        var snapshot = Snapshot<TAggregate>.Create(aggregate, storeEvent);
                        await eventStore.AppendAsync(snapshot, ct);
                    }

                    await eventBusGateway.PublishAsync(@event, ct);
                }

            },
            cancellationToken: cancellationToken);

        public IAsyncEnumerable<Guid> StreamAggregatesId<TAggregate>()
            where TAggregate : IAggregateRoot
            => eventStore.StreamAggregatesId<TAggregate>();

        public Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken)
            => eventBusGateway.PublishAsync(@event, cancellationToken);

        public Task SchedulePublishAsync(IDelayedEvent @event, DateTimeOffset scheduledTime, CancellationToken cancellationToken)
            => eventBusGateway.SchedulePublishAsync(@event, scheduledTime, cancellationToken);
    }
}