using MassTransit;

namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using MassTransit;
    using Student.Application.UseCases.Events;
    using Student.Domain;
    public class ProjectLessonProgressWhenLessonProgressChangedConsumer(
        IProjectLessonProgressWhenLessonProgressChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.LessonProgressCreated>,
        IConsumer<DomainEvent.LessonProgressFinishedStatus>
    {
        public Task Consume(ConsumeContext<DomainEvent.LessonProgressCreated> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.LessonProgressFinishedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
