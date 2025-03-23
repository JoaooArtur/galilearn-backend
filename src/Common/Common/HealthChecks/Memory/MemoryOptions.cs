namespace Common.HealthChecks.Memory
{
    public class MemoryOptions
    {
        public long Threshold { get; set; } = Convert.ToInt64(Math.Pow(1024, 4));
    }
}
