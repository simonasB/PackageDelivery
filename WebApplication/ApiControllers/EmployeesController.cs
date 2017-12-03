using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PackageDelivery.Core;
using PackageDelivery.Domain.Dtos;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Filters;
using PackageDelivery.WebApplication.Models;
using PackageDelivery.WebApplication.Models.AccountViewModels;
using PackageDelivery.WebApplication.Models.Api;

namespace PackageDelivery.WebApplication.ApiControllers {
    [AllowAnonymous]
    [Route("api/Users")]
    [ValidateModel]
    public class EmployeesController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly IPasswordValidator<User> _passValidator;
        private readonly IPasswordHasher<User> _passHasher;

        public EmployeesController(UserManager<User> userManager,
            IUserValidator<User> userValidator, IPasswordValidator<User> passValidator,
            IPasswordHasher<User> passHasher) {
            _userManager = userManager;
            _userValidator = userValidator;
            _passValidator = passValidator;
            _passHasher = passHasher;
        }

        private readonly User testUser = new User {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test"
        };

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostUser([FromBody] EmployeeViewModel model) {
            var user = new User {UserName = model.Email, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) {
                var createdUser = await _userManager.FindByEmailAsync(model.Email);
                if (model.IsAdmin) {
                    await _userManager.AddToRoleAsync(createdUser, UserRoles.ADMIN);
                }

                if (model.IsDriver) {
                    await _userManager.AddToRoleAsync(createdUser, UserRoles.DRIVER);
                }

                if (model.IsManager) {
                    await _userManager.AddToRoleAsync(createdUser, UserRoles.MANAGER);
                }

                return Created("", model);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> PutUser(string userId, EmployeeViewModel model) {
            User user = await _userManager.FindByIdAsync(userId);

            if (user != null) {
                // Validate UserName and Email 
                user.UserName =
                    model.Email; // UserName won't be changed in the database until UpdateAsync is executed successfully
                user.Email = model.Email;
                IdentityResult validUserResult = await _userValidator.ValidateAsync(_userManager, user);
                if (!validUserResult.Succeeded) {
                    return BadRequest();
                }

                // Validate password
                // Step 1: using built in validations
                IdentityResult passwordResult = await _userManager.CreateAsync(testUser, model.Password);
                if (passwordResult.Succeeded) {
                    await _userManager.DeleteAsync(testUser);
                } else {
                    return BadRequest();
                }
                /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator. 
                   So the built in password validation stop working here */
                IdentityResult validPasswordResult =
                    await _passValidator.ValidateAsync(_userManager, user, model.Password);
                if (validPasswordResult.Succeeded) {
                    user.PasswordHash = _passHasher.HashPassword(user, model.Password);
                } else {
                    return BadRequest();
                }

                // Update user info
                if (validUserResult.Succeeded && passwordResult.Succeeded && validPasswordResult.Succeeded) {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {

                        if (model.IsAdmin) {
                            await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);
                        }

                        if (model.IsDriver) {
                            await _userManager.AddToRoleAsync(user, UserRoles.DRIVER);
                        }

                        if (model.IsManager) {
                            await _userManager.AddToRoleAsync(user, UserRoles.MANAGER);
                        }

                        return NoContent();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId) {
            User user = await _userManager.FindByIdAsync(userId);

            if (user != null) {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) {
                    return Ok();
                }
                return BadRequest();
            }
            return NotFound();
        }
    }
}
