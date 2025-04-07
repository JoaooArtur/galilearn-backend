
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Options
{
    public record JwtOptions
    {
        [Required]
        public required string Key { get; init; }

        [Required]
        public required string Issuer { get; init; }

        [Required]
        public required List<string> Policies { get; init; }

        [Required]
        public required string Audience { get; init; }
    }
}
