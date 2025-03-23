using Microsoft.Extensions.Options;

namespace WebBff.Middlewares;

public class ContentValidationMiddleware(RequestDelegate next, IOptions<ContentValidationOptions> options)
{
    private readonly RequestDelegate _next = next;
    private readonly string _contentValidation = options.Value.ContentValidation;
    private readonly string _path = options.Value.Path;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentType == "application/json; charset=UTF-8" 
            && context.Request.Path.Value.Contains(_path))
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (body.Contains(_contentValidation))
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }
        }
        
        await _next(context);
    }
}


public class ContentValidationOptions
{
    public string ContentValidation { get; set; }
    public string Path { get; set; }
}