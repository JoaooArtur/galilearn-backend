namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using MassTransit;
    using Student.Application.UseCases.Events;
    using Student.Domain;
    public class ProjectSubjectProgressWhenSubjectProgressChangedConsumer(
        IProjectSubjectProgressWhenSubjectProgressChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.SubjectProgressCreated>,
        IConsumer<DomainEvent.SubjectProgressFinishedStatus>
    {
        public Task Consume(ConsumeContext<DomainEvent.SubjectProgressCreated> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.SubjectProgressFinishedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
