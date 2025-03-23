using MassTransit;

namespace Core.Infrastructure.EventBus
{
    public interface IEventReceiveEndpointConfiguration
    {
        void AddEventReceiveEndpoints(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator, IRegistrationContext registrationContext);
    }
}
