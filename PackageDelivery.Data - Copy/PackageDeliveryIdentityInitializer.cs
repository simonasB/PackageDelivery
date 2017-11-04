using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Data {
    public class PackageDeliveryIdentityInitializer {
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly UserManager<User> _userMgr;

        public PackageDeliveryIdentityInitializer(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr) {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed() {
            var user = await _userMgr.FindByNameAsync("a@b.com");

            // Add User
            if (user == null) {
                if (!(await _roleMgr.RoleExistsAsync("Admin"))) {
                    var role = new IdentityRole("Admin");
                    role.Claims.Add(new IdentityRoleClaim<string>() {ClaimType = "IsAdmin", ClaimValue = "True"});
                    await _roleMgr.CreateAsync(role);
                }

                user = new User {
                    UserName = "a@b.com",
                    FirstName = "a",
                    LastName = "b",
                    Email = "a@b.com",
                    IsActive = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,                  
                };

                var userResult = await _userMgr.CreateAsync(user, "P@assw0rd!");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded) {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }
    }
}
