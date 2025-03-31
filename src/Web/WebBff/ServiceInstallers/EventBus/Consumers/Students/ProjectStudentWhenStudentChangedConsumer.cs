namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using MassTransit;
    using Student.Application.UseCases.Events;
    using Student.Domain;
    public class ProjectStudentWhenStudentChangedConsumer(
        IProjectStudentWhenStudentChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.StudentCreated>,
        IConsumer<DomainEvent.StudentActiveStatus>,
        IConsumer<DomainEvent.StudentBlockedStatus>,
        IConsumer<DomainEvent.StudentDefaultStatus>,
        IConsumer<DomainEvent.FriendAdded>,
        IConsumer<DomainEvent.StudentDeleted>
    {
        public Task Consume(ConsumeContext<DomainEvent.StudentCreated> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.StudentActiveStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.StudentBlockedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.StudentDefaultStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.StudentDeleted> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.FriendAdded> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
