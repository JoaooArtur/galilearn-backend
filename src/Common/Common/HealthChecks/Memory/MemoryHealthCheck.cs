using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Common.HealthChecks.Memory
{
    public class MemoryHealthCheck(IOptionsMonitor<MemoryOptions> options) : IHealthCheck
    {
        private readonly IOptionsMonitor<MemoryOptions> _options = options;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var options = _options.Get(context.Registration.Name);

            var allocated = GC.GetTotalMemory(forceFullCollection: false);

            var data = new Dictionary<string, object>()
            {
                ["Allocated"] = allocated,
                ["Gen0Collections"] = GC.CollectionCount(0),
                ["Gen1Collections"] = GC.CollectionCount(1),
                ["Gen2Collections"] = GC.CollectionCount(2)
            };

            var status = allocated >= options.Threshold ? context.Registration.FailureStatus : HealthStatus.Healthy;

            var healthCheckResult = new HealthCheckResult(status, description: $"Reports degraded status if allocated bytes >= {options.Threshold} bytes", data: data);

            return Task.FromResult(healthCheckResult);
        }
    }
}
