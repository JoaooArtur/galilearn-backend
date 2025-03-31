using Common.DependencyInjection.Extensions;
using Core.Infrastructure.Extensions;
using CorrelationId;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using WebBff.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services
    .InstallServicesFromAssemblies(
        builder.Configuration,
        WebBff.AssemblyReference.Assembly,
        Core.Persistence.AssemblyReference.Assembly)
    .InstallModulesFromAssemblies(
        builder.Configuration,
        Student.Infrastructure.AssemblyReference.Assembly,
        Subject.Infrastructure.AssemblyReference.Assembly);

#if !DEBUG
builder.ConfigureSystemsManager();
#endif

builder.Services
    .AddHttpContextAccessor();

builder
    .ConfigureLogging(builder.Configuration)
    .ConfigureServiceProvider()
    .ConfigureAppConfiguration();

builder.Services
    .AddCorrelationId();

builder.Services
    .AddEndpointsApiExplorer();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddHealthCheck(builder.Configuration);

//builder.Services.AddAuthentication("BmpBasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BmpBasicAuthentication", null);

#if !DEBUG
var port = "8080";
#else
var port = "5171";
#endif

builder.Services
    .AddHealthChecksUI(setupSettings: setup =>
    {
        setup.SetEvaluationTimeInSeconds(5);
        setup.SetApiMaxActiveRequests(3);
        setup.AddHealthCheckEndpoint(AppDomain.CurrentDomain.FriendlyName, $"http://localhost:{port}/api/healthz");
        setup.MaximumHistoryEntriesPerEndpoint(60);

    }).AddInMemoryStorage();

builder.Services.AddHttpLogging(options
    => options.LoggingFields = HttpLoggingFields.All);

builder.Services.AddCommonServiceCollection(builder.Configuration);

builder.Services
    .AddControllers()
    .AddApplicationPart(WebBff.AssemblyReference.Assembly);

var app = builder.Build();

app.UsePathBase("/api");
app.UseStaticFiles();

app.MapHealthChecks("/api/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).ShortCircuit();

app.UseHealthChecksUI(config =>
{
    config.UIPath = "/monitoring";
});

app.UseSwagger()
   .UseSwaggerUI()
   .UseCors(corsPolicyBuilder =>
        corsPolicyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

app.UseCorrelationId();
app.UseAuthorization();
app.UseSerilogRequestLogging()
   .UseHttpsRedirection();

app.UseMiddlewares();

app.MapControllers();

try
{
    app.UseRecurringJobs();

    await app.RunAsync();

    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
