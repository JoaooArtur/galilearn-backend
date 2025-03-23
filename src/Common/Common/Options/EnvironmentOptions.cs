using System.ComponentModel.DataAnnotations;

namespace Common.Options
{
    public sealed record EnvironmentOptions
    {
        [Required]
        public string EnvironmentName { get; init; }
    }
}
