using Microsoft.Extensions.Options;

namespace WebBff.Handlers.Options
{
    /// <summary>
    /// Represents the <see cref="CartaosSimplesBasicAuthenticationOptionsSetup"/> setup.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CartaosSimplesBasicAuthenticationOptionsSetup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    public class CartaosSimplesBasicAuthenticationOptionsSetup(IConfiguration configuration) : IConfigureOptions<CartaosSimplesBasicAuthenticationOptions>
    {
        private const string ConfigurationSectionName = "CartaosSimplesBasicAuthenticationOptions";

        /// <inheritdoc />
        public void Configure(CartaosSimplesBasicAuthenticationOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
