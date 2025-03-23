using Masking.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace WebBff.Extensions
{
    /// <summary>
    /// Contains extensions methods for the <see cref="IHostBuilder"/> interface.
    /// </summary>
    internal static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures Serilog as a logging providers using the configuration defined in the application settings.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        public static IHostBuilder ConfigureLogging(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            builder.Host.UseSerilog((context, services, logger) =>
            {
                logger
                    .Destructure.ByMaskingProperties("Password")
                    .Enrich.WithCorrelationId()
                    .Enrich.WithRequestUserId()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithRequestUserId()
                    .Enrich.WithThreadId()
                    .Enrich.WithThreadName()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
                    .Enrich.FromLogContext()
                    .MinimumLevel.Override("MassTransit", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
#if !DEBUG
                    .WriteTo.DatadogLogs(
                        configuration.GetValue<string>("Datadog:ApiKey"),
                        service: "webbff",
                        tags: ["service:webbff"]);

#endif
#if DEBUG
                    .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
                    .WriteTo.Seq("http://localhost:5341");
#endif
            });

            return builder.Host;
        }

        /// <summary>
        /// Configures the service provider settings for the application, ensuring proper validation
        /// of scopes and service configurations during development.
        /// </summary>
        /// <param name="builder">The host builder to configure.</param>
        public static IHostBuilder ConfigureServiceProvider(this IHostBuilder builder) =>
            builder.UseDefaultServiceProvider((context, provider) =>
                provider.ValidateScopes =
                provider.ValidateOnBuild =
                context.HostingEnvironment.IsDevelopment());

        /// <summary>
        /// Configures the application's configuration sources, including user secrets and environment variables.
        /// </summary>
        /// <param name="builder">The host builder to configure.</param>
        public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder builder) =>
            builder.ConfigureAppConfiguration(configuration =>
                configuration
                    .AddUserSecrets<Program>()
                    .AddEnvironmentVariables());

    }
}
