using Core.Application.EventBus;
using Core.Application.Time;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.EventBus;
using Core.Infrastructure.Time;
using Core.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Student.Infrastructure.ServiceInstallers
{
    /// <summary>
    /// Represents the users module infrastructure service installer.
    /// </summary>
    internal sealed class InfrastructureServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration) =>
            services
                .Tap(services.TryAddTransient<ISystemTime, SystemTime>)
                .Tap(services.TryAddTransient<IEventBus, EventBus>);
    }
}
