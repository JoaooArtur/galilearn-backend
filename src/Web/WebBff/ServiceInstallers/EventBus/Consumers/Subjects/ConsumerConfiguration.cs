using Core.Infrastructure.EventBus;
using MassTransit;
using WebBff.ServiceInstallers.EventBus.Extensions;

namespace WebBff.ServiceInstallers.EventBus.Consumers.Subjects
{
    using SubjectDomainEvent = Subject.Domain.DomainEvent;
    /// <summary>
    /// Represents the consumer configuration for the subject module.
    /// </summary>
    internal sealed class ConsumerConfiguration : IEventReceiveEndpointConfiguration
    {
        private const string MODULE_NAME = "subject";

        public void AddEventReceiveEndpoints(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator, IRegistrationContext registrationContext)
        {
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, SubjectDomainEvent.SubjectCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, SubjectDomainEvent.SubjectDeleted>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectWhenSubjectChangedConsumer, SubjectDomainEvent.LessonAdded>(registrationContext, MODULE_NAME);
        }
    }
}
