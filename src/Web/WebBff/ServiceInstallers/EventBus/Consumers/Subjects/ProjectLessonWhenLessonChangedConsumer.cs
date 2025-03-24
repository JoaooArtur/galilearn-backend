namespace WebBff.ServiceInstallers.EventBus.Consumers.Subjects
{
    using MassTransit;
    using Subject.Application.UseCases.Events;
    using Subject.Domain;
    public class ProjectLessonWhenLessonChangedConsumer(
        IProjectLessonWhenLessonChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.LessonAdded>
    {
        public Task Consume(ConsumeContext<DomainEvent.LessonAdded> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
