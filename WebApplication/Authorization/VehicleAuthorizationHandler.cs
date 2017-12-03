using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PackageDelivery.Core;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Authorization
{
    public class VehicleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Vehicle>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            Vehicle resource) {
            if (context.User == null || resource == null) {
                return Task.FromResult(0);
            }

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName) {
                return Task.FromResult(0);
            }

            if (requirement.Name != Constants.ReadOperationName &&
                context.User.Claims.Single(o => o.ValueType == Claims.Role).Value != UserRoles.ADMIN) {
                context.Fail();
            } else {
                context.Succeed(requirement);
            }
               
            return Task.FromResult(0);
        }
    }
}
