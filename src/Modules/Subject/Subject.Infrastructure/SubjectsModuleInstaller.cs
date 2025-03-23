using Core.Infrastructure.Configuration;
using Core.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Subject.Infrastructure
{
    /// <summary>
    /// Represents the students module installer.
    /// </summary>
    public sealed class SubjectsModuleInstaller : IModuleInstaller
    {
        /// <inheritdoc />
        public void Install(IServiceCollection services, IConfiguration configuration) =>
            services
                .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
                .AddTransientAsMatchingInterfaces(AssemblyReference.Assembly)
                .AddTransientAsMatchingInterfaces(Persistence.AssemblyReference.Assembly)
                .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
                .AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
    }
}
