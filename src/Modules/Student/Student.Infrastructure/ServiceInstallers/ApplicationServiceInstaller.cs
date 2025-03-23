using Core.Application.Behaviors;
using Core.Infrastructure.Configuration;
using Core.Shared.Extensions;
using Student.Application.Services;
using Student.Infrastructure.Services;
using Student.Application.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Student.Infrastructure.ServiceInstallers
{
    /// <summary>
    /// Represents the customers module application service installer.
    /// </summary>
    internal sealed class ApplicationServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration) =>
            services
                .Tap(services.TryAddTransient<IStudentApplicationService, StudentApplicationService>)
                .AddEventInteractors()
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);

                    config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
                    config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                })
                .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);
    }
}
