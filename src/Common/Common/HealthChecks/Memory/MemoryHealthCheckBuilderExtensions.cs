using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Common.HealthChecks.Memory
{
    public static class MemoryHealthCheckBuilderExtensions
    {
        private const string Name = "Memory";
        private const HealthStatus Degraded = HealthStatus.Degraded;

        public static IHealthChecksBuilder AddMemory(
            this IHealthChecksBuilder builder, 
            string name = null,
            HealthStatus? failureStatus = null, 
            IEnumerable<string> tags = null, 
            long? thresholdInBytes = null)
        {
            builder.AddCheck<MemoryHealthCheck>(name ?? Name, failureStatus ?? Degraded, tags);

            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<MemoryOptions>(name, options => options.Threshold = thresholdInBytes.Value);
            }

            return builder;
        }
    }
}
