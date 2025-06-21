using Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureOptions<EnvironmentOptionsSetup>();

            return services;
        }
    }
}
