using Microsoft.Extensions.Options;

namespace WebBff.Handlers.Options
{
    /// <summary>
    /// Represents the <see cref="B2eBasicAuthenticationOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="B2eBasicAuthenticationOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    internal sealed class B2eBasicAuthenticationOptionsSetup(IConfiguration configuration) : IConfigureOptions<B2eBasicAuthenticationOptions>
    {
        private const string ConfigurationSectionName = "B2eBasicAuthenticationOptions";

        /// <inheritdoc />
        public void Configure(B2eBasicAuthenticationOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
