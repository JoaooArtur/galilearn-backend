using Microsoft.AspNetCore.Authorization;

namespace Core.Application.Requirements
{
    public class HasRoleRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public List<string> Roles { get; }

        public HasRoleRequirement(List<string> roles, string issuer)
        {
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
