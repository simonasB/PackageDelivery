using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageDelivery.Domain.Dtos;
using PackageDelivery.UI.Api;

namespace PackageDelivery.UI.Controllers {
    public class AccountController : Controller {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService) {
            _apiService = apiService;
        }

        [Authorize]
        public async Task Logout() {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        [Authorize]
        public IActionResult Login() {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(AccountViewModel accountViewModel) {
            if (ModelState.IsValid) {
                var result = await _apiService.Post("Account", accountViewModel);
                if (result == null) {
                    // Log error
                }

                return View("SuccessfulRegistration");
            }

            return View(accountViewModel);
        }
    }
}
