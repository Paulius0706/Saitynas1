using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TimeT.Auth.Model;
using Microsoft.IdentityModel.JsonWebTokens;

namespace TimeT.Auth
{
    public class ResourceOwnerAuthHandler : AuthorizationHandler<ResourceOwnerRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement,
        IUserOwnedResource resource)
        {
            if (context.User.IsInRole(UserRoles.Admin) ||
                context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) == resource.userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public record ResourceOwnerRequirement : IAuthorizationRequirement;
}
