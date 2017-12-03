using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PackageDelivery.Core;
using PackageDelivery.Domain.Dtos;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Authorization;
using PackageDelivery.WebApplication.Filters;
using PackageDelivery.WebApplication.Models;
using PackageDelivery.WebApplication.Models.Api;

namespace PackageDelivery.WebApplication.ApiControllers {
    [Route("api/Account")]
    [ValidateModel]
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly IPasswordValidator<User> _passValidator;
        private readonly IPasswordHasher<User> _passHasher;

        public AccountController(UserManager<User> userManager,
            IUserValidator<User> userValidator, IPasswordValidator<User> passValidator,
            IPasswordHasher<User> passHasher) {
            _userManager = userManager;
            _userValidator = userValidator;
            _passValidator = passValidator;
            _passHasher = passHasher;
        }

        private readonly User _testUser = new User {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test"
        };

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] AccountViewModel model) {
            if (await _userManager.FindByEmailAsync(model.Email) != null) {
                return BadRequest("User with the same email already exists.");
            }

            var user = new User {UserName = model.Email, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) {
                model.UserId = user.Id;
                await _userManager.AddClaimAsync(user, new Claim(Claims.Role, UserRoles.CUSTOMER));
                return Created("", model);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{userId}")]
        [Authorize(Policy = Policy.AccountOwner)]
        public async Task<IActionResult> Put(string userId, [FromBody] AccountViewModel model) {
            User user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // Validate UserName and Email 
                if (user.Email != model.Email && await _userManager.FindByEmailAsync(model.Email) != null) { // UserName won't be changed in the database until UpdateAsync is executed successfully
                    return BadRequest("User with the same email already exists.");
                }
                user.UserName = model.Email;
                user.Email = model.Email;
                IdentityResult validUserResult = await _userValidator.ValidateAsync(_userManager, user);
                if (!validUserResult.Succeeded) {
                    return BadRequest();
                }

                // Validate password
                // Step 1: using built in validations
                IdentityResult passwordResult = await _userManager.CreateAsync(_testUser, model.Password);
                if (passwordResult.Succeeded)
                {
                    await _userManager.DeleteAsync(_testUser);
                }
                else {
                    return BadRequest();
                }
                /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator. 
                   So the built in password validation stop working here */
                IdentityResult validPasswordResult = await _passValidator.ValidateAsync(_userManager, user, model.Password);
                if (validPasswordResult.Succeeded)
                {
                    user.PasswordHash = _passHasher.HashPassword(user, model.Password);
                }
                else {
                    return BadRequest();
                }

                // Update user info
                if (validUserResult.Succeeded && passwordResult.Succeeded && validPasswordResult.Succeeded) {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
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
        [Authorize(Policy = Policy.AccountOwner)]
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
