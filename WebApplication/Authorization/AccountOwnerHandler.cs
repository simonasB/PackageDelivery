using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PackageDelivery.WebApplication.Authorization
{
    public class AccountOwnerHandler : AuthorizationHandler<AccountOwnerRequirement>
    {
        private readonly IActionContextAccessor _accessor;

        public AccountOwnerHandler(IActionContextAccessor accessor) {
            _accessor = accessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AccountOwnerRequirement requirement) {
            var userId = (string) _accessor.ActionContext.RouteData.Values["userId"];

            if (context.User.Claims.FirstOrDefault(o => o.Type == Claims.NameIdentifier)?.Value == userId) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
            return Task.FromResult(0);
        }
    }
}
