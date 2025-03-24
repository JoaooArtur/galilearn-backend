namespace WebBff.ServiceInstallers.EventBus.Consumers.Subjects
{
    using MassTransit;
    using Subject.Application.UseCases.Events;
    using Subject.Domain;
    public class ProjectQuestionWhenQuestionChangedConsumer(
        IProjectQuestionWhenQuestionChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.QuestionAdded>,
        IConsumer<DomainEvent.AnswerOptionAdded>
    {
        public Task Consume(ConsumeContext<DomainEvent.QuestionAdded> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.AnswerOptionAdded> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
