
namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using MassTransit;
    using Student.Application.UseCases.Events;
    using Student.Domain;
    public class ProjectAttemptWhenAttemptChangedConsumer(
        IProjectAttemptWhenAttemptChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.AttemptCreated>,
        IConsumer<DomainEvent.AttemptAnswered>,
        IConsumer<DomainEvent.AttemptFinishedStatus>,
        IConsumer<DomainEvent.AttemptInProgressStatus>
    {
        public Task Consume(ConsumeContext<DomainEvent.AttemptCreated> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.AttemptAnswered> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.AttemptFinishedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.AttemptInProgressStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
