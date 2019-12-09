using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspServer.Authorization
{
    public class ValidateSecurityStampHandler : AuthorizationHandler<ValidateSecurityStampRequirement>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateSecurityStampHandler(SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidateSecurityStampRequirement requirement)
        {
            ClaimsPrincipal claimsPrincipal = context.User;

            bool userExists = (await _signInManager.ValidateSecurityStampAsync(claimsPrincipal)) != null;

            if (userExists)
                context.Succeed(requirement);
            else
            {
                var response = _httpContextAccessor.HttpContext.Response;

                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await response.Body.FlushAsync();
            }
        }
    }
}
