using Common.Options;
using Microsoft.Extensions.Options;

namespace WebBff.Extensions
{
    public static class HostExtensions
    {
        public static void UseRecurringJobs(this WebApplication app)
        {
            var host = app.Services.GetRequiredService<IHost>();
            var environmentOptions = app.Services.GetRequiredService<IOptions<EnvironmentOptions>>();

            //host.UseStudentRecurringJobs();
        }
    }
}
