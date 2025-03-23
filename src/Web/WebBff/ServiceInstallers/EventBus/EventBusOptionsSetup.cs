using Microsoft.Extensions.Options;
using WebBff.ServiceInstallers.EventBus.Options;

namespace WebBff.ServiceInstallers.EventBus
{
    /// <summary>
    /// Represents the <see cref="EventBusOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EventBusOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    internal sealed class EventBusOptionsSetup(IConfiguration configuration) : IConfigureOptions<EventBusOptions>
    {
        private const string ConfigurationSectionName = "EventBusOptions";

        /// <inheritdoc />
        public void Configure(EventBusOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
