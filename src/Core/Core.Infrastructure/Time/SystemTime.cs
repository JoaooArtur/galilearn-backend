using Core.Application.Time;

namespace Core.Infrastructure.Time
{
    /// <summary>
    /// Represents the system time interface.
    /// </summary>
    public sealed class SystemTime : ISystemTime
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
