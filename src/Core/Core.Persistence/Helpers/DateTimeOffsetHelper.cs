using System.Runtime.InteropServices;

namespace Core.Persistence.Helpers
{
    public static class DateTimeOffsetHelper
    {
        private static readonly string SouthAmericaZoneId = "E. South America Standard Time";
        private static readonly string SaoPauloZoneId = "America/Sao_Paulo";

        public static DateTimeOffset ConvertToBrazilianOffset(DateTime utcDateTime)
        {
            var brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? SouthAmericaZoneId : SaoPauloZoneId);

            var brazilTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, brazilTimeZone);

            return new DateTimeOffset(brazilTime, brazilTimeZone.GetUtcOffset(brazilTime));
        }
    }
}
