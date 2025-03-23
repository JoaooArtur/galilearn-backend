using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebBff.ServiceInstallers.Swagger
{
    /// <summary>
    /// Represents the <see cref="SwaggerUIOptions"/> setup.
    /// </summary>
    internal sealed class SwaggerUiOptionsSetup(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerUIOptions>
    {
        /// <inheritdoc />
        public void Configure(SwaggerUIOptions options)
        {
            options.RoutePrefix = "swagger";

            foreach (var version in provider.ApiVersionDescriptions.Select(version => version.GroupName))
            {
                options.SwaggerEndpoint($"/api/swagger/{version}/swagger.json", version);
            }

            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.EnableValidator();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        }
    }
}
