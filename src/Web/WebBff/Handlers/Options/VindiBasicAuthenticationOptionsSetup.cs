using Microsoft.Extensions.Options;

namespace WebBff.Handlers.Options
{
    /// <summary>
    /// Represents the <see cref="VindiBasicAuthenticationOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VindiBasicAuthenticationOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    internal sealed class VindiBasicAuthenticationOptionsSetup(IConfiguration configuration) : IConfigureOptions<VindiBasicAuthenticationOptions>
    {
        private const string ConfigurationSectionName = "VindiBasicAuthenticationOptions";

        /// <inheritdoc />
        public void Configure(VindiBasicAuthenticationOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
