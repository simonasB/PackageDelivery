using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PackageDelivery.Core;

namespace PackageDelivery.WebApplication.Authorization
{
    public class CompanyMemberHandler : AuthorizationHandler<CompanyMemberRequirement> {
        private readonly IActionContextAccessor _accessor; // for getting teamId from RouteData

        public CompanyMemberHandler(IActionContextAccessor accessor) {
            _accessor = accessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyMemberRequirement requirement) {
            var companyId = int.Parse((string)_accessor.ActionContext.RouteData.Values["companyId"]);
            
            if (int.Parse(context.User.Claims.FirstOrDefault(o => o.Type == Claims.CompanyId)?.Value ?? "-1") == companyId) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
            return Task.FromResult(0);
        }
    }
}
