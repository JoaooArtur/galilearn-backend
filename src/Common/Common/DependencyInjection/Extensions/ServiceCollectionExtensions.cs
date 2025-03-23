using Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string CUSTOMER_PROJECTION_DATABASE_NAME = "Customer";
        private const string ORDER_PROJECTION_DATABASE_NAME = "Order";

        public static IServiceCollection AddCommonServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            //services
            //    .AddIntegrationBmp()
            //    .AddIntegrationVindi()
            //    .AddCommunication()
            //    .AddAuth0Api()
            //    .AddIntegrationB2e()
            //    .AddIntegrationCaf(configuration.GetSection("Projections").GetValue<string>("Order"))
            //    .AddIntegrationClaro(configuration.GetSection("Projections").GetValue<string>("Customer"));

            //services
            //    .AddAuthAuthentication();

            services
                .ConfigureOptions<EnvironmentOptionsSetup>();

            return services;
        }
    }
}
