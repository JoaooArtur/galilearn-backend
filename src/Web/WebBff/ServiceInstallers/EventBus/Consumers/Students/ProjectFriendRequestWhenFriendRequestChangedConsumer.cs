using MassTransit;

namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using MassTransit;
    using Student.Application.UseCases.Events;
    using Student.Domain;
    public class ProjectFriendRequestWhenFriendRequestChangedConsumer(
        IProjectFriendRequestWhenFriendRequestChangedEventHandler eventHandler) :
        IConsumer<DomainEvent.FriendRequestRejectedStatus>,
        IConsumer<DomainEvent.FriendRequestAcceptedStatus>,
        IConsumer<DomainEvent.FriendRequestCreatedStatus>
    {
        public Task Consume(ConsumeContext<DomainEvent.FriendRequestCreatedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.FriendRequestAcceptedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
        public Task Consume(ConsumeContext<DomainEvent.FriendRequestRejectedStatus> context)
            => eventHandler.Handle(context.Message, context.CancellationToken);
    }
}
