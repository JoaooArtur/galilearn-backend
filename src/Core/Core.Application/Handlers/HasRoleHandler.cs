using Core.Application.Options;
using Core.Application.Requirements;
using Discord;
using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Handlers
{
    public class HasRoleHandler(IOptionsSnapshot<JwtOptions> option) : AuthorizationHandler<HasRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            var jwtOptions = option.Value;

            var roles = context.User.Claims.Where(c => c.Type == "Role");

            if (roles.Any(r => requirement.Roles.Contains(r.Value)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
