using WebBff.Middlewares;

namespace WebBff.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app)
        => app.UseMiddleware<ContentValidationMiddleware>();
}
