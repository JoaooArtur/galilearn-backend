using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace WebBff.ServiceInstallers.Swagger
{
    internal sealed class SwaggerOptionsSetup : IConfigureOptions<SwaggerOptions>
    {
        /// <inheritdoc />
        public void Configure(SwaggerOptions options)
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        }
    }
}
