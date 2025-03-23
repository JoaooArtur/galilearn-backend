using Core.Application.EventBus;
using Core.Domain.Primitives;
using MassTransit;

namespace Core.Infrastructure.EventBus
{
    public sealed class EventConsumer<TIHandler, TEvent>(TIHandler handler) : IConsumer<TEvent>
        where TEvent : class, IEvent
        where TIHandler : IEventHandler<TEvent>
    {
        public Task Consume(ConsumeContext<TEvent> context)
            => handler.Handle(context.Message, context.CancellationToken);
    }
}
