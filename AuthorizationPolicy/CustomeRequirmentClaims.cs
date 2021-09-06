using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample.AuthorizationPolicy
{
    public class CustomeRequirmentClaims : IAuthorizationRequirement
    {
        public string claimType;

        public CustomeRequirmentClaims(string claimType)
        {
            this.claimType = claimType;
        }

    }

    public class CustomeRequirmentClaimsHandler : AuthorizationHandler<CustomeRequirmentClaims>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomeRequirmentClaims requirement)
        {
            var hasClaims = context.User.Claims.Any(c => c.Type == requirement.claimType);
            if (hasClaims)
            {
                context.Succeed(requirement);
            }
        
            return Task.CompletedTask;

        }
}
}
