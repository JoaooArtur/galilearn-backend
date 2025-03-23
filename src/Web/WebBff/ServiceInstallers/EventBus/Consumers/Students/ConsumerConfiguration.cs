using Core.Infrastructure.EventBus;
using MassTransit;
using WebBff.ServiceInstallers.EventBus.Extensions;

namespace WebBff.ServiceInstallers.EventBus.Consumers.Students
{
    using StudentDomainEvent = Student.Domain.DomainEvent;
    /// <summary>
    /// Represents the consumer configuration for the student module.
    /// </summary>
    internal sealed class ConsumerConfiguration : IEventReceiveEndpointConfiguration
    {
        private const string MODULE_NAME = "student";

        public void AddEventReceiveEndpoints(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator, IRegistrationContext registrationContext)
        {
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.StudentCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.StudentActiveStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.StudentBlockedStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.StudentDefaultStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.StudentDeleted>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, StudentDomainEvent.FriendAdded>(registrationContext, MODULE_NAME);
        }
    }
}
