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
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.StudentCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.StudentActiveStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.StudentBlockedStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.StudentDefaultStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.StudentDeleted>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectStudentWhenStudentChangedConsumer, StudentDomainEvent.FriendAdded>(registrationContext, MODULE_NAME);

            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectAttemptWhenAttemptChangedConsumer, StudentDomainEvent.AttemptCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectAttemptWhenAttemptChangedConsumer, StudentDomainEvent.AttemptAnswered>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectAttemptWhenAttemptChangedConsumer, StudentDomainEvent.AttemptFinishedStatus>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectAttemptWhenAttemptChangedConsumer, StudentDomainEvent.AttemptInProgressStatus>(registrationContext, MODULE_NAME);

            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectLessonProgressWhenLessonProgressChangedConsumer, StudentDomainEvent.LessonProgressCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectLessonProgressWhenLessonProgressChangedConsumer, StudentDomainEvent.LessonProgressFinishedStatus>(registrationContext, MODULE_NAME);

            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectProgressWhenSubjectProgressChangedConsumer, StudentDomainEvent.SubjectProgressCreated>(registrationContext, MODULE_NAME);
            rabbitMqBusFactoryConfigurator.ConfigureEventReceiveEndpoint<ProjectSubjectProgressWhenSubjectProgressChangedConsumer, StudentDomainEvent.SubjectProgressFinishedStatus>(registrationContext, MODULE_NAME);
        }
    }
}
