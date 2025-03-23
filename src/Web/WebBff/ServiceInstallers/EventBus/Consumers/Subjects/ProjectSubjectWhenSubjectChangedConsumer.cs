namespace WebBff.ServiceInstallers.EventBus.Consumers.Subjects
{
    using MassTransit;
    using Subject.Application.UseCases.Events;
    using Subject.Domain;
    public class ProjectSubjectWhenSubjectChangedConsumer(
        IProjectSubjectWhenSubjectChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.SubjectCreated>,
        IConsumer<DomainEvent.LessonAdded>,
        IConsumer<DomainEvent.SubjectDeleted>
    {
        public Task Consume(ConsumeContext<DomainEvent.SubjectCreated> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.LessonAdded> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.SubjectDeleted> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
