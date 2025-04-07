using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Endpoints.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="ClaimsPrincipal"/> class.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        public static Guid ExtractIdFromToken(string token)
            => Guid.Parse(token.GetIdentityProviderId("Id"));
        

        /// <summary>
        /// Gets the identity provider identifier of the currently authenticated user.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The identity provider identifier of the currently authenticated user if it exists, or an empty string.</returns>
        internal static string GetIdentityProviderId(this string jwtToken, string claimName)
            => new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Claims.SingleOrDefault(claim => claim.Type == claimName)?.Value ?? string.Empty;
    }
}
