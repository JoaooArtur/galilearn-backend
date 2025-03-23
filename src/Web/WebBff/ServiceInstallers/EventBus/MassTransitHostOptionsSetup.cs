using MassTransit;
using Microsoft.Extensions.Options;

namespace WebBff.ServiceInstallers.EventBus
{
    /// <summary>
    /// Represents the <see cref="MassTransitHostOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MassTransitHostOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    internal sealed class MassTransitHostOptionsSetup(IConfiguration configuration) : IConfigureOptions<MassTransitHostOptions>
    {
        private const string ConfigurationSectionName = "MassTransitHostOptions";

        /// <inheritdoc />
        public void Configure(MassTransitHostOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
