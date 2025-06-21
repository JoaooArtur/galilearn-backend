﻿using Core.Domain.Primitives;
using Core.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public interface IApplicationService<TContext> where TContext : DbContext //NOSONAR
    {
        Task AppendEventsAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot;

        Task<Result<TAggregate>> LoadAggregateAsync<TAggregate>(Guid id, CancellationToken cancellationToken)
            where TAggregate : class, IAggregateRoot, new();

        IAsyncEnumerable<Guid> StreamAggregatesId<TAggregate>()
            where TAggregate : IAggregateRoot;

        Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken);
        
        Task SchedulePublishAsync(IDelayedEvent @event, DateTimeOffset scheduledTime, CancellationToken cancellationToken);
    }
}
