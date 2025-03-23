using Core.Infrastructure.Configuration;

namespace WebBff.ServiceInstallers.Swagger
{
    /// <summary>
    /// Represents the swagger service installer.
    /// </summary>
    internal sealed class SwaggerServiceInstaller : IServiceInstaller
    {
        /// <inheritdoc />
        void IServiceInstaller.Install(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<SwaggerOptionsSetup>();
            services.ConfigureOptions<SwaggerGenOptionsSetup>();
            services.ConfigureOptions<SwaggerUiOptionsSetup>();

            services.AddSwaggerGen();
        }
    }
}
