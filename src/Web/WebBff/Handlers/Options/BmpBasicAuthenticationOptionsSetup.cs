using Microsoft.Extensions.Options;

namespace WebBff.Handlers.Options
{
    /// <summary>
    /// Represents the <see cref="BmpBasicAuthenticationOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BmpBasicAuthenticationOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    internal sealed class BmpBasicAuthenticationOptionsSetup(IConfiguration configuration) : IConfigureOptions<BmpBasicAuthenticationOptions>
    {
        private const string ConfigurationSectionName = "BmpBasicAuthenticationOptions";

        /// <inheritdoc />
        public void Configure(BmpBasicAuthenticationOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
