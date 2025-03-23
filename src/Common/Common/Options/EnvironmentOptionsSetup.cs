using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.Options
{
    internal class EnvironmentOptionsSetup(IConfiguration configuration) : IConfigureOptions<EnvironmentOptions>
    {
        private const string ConfigurationSectionName = "EnvironmentOptions";

        public void Configure(EnvironmentOptions options) => configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
