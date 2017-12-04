using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PackageDelivery.Domain.Dtos.CompanyDtos;
using PackageDelivery.Domain.Dtos.CurrencyDtos;
using PackageDelivery.Domain.Dtos.VehicleMakeDtos;
using PackageDelivery.UI.Api;

namespace PackageDelivery.UI.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly IApiService _apiService;
        private const string ApiUri = "Companies";

        public CompanyController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: VehicleMakes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var companyDto = await _apiService.Get<CompanyDto>($"{ApiUri}/{id}");

            if (companyDto == null)
            {
                return NotFound();
            }

            return View(companyDto);
        }

        // GET: VehicleMakes/Create
        public async Task<IActionResult> Create() {
            var currencies = await _apiService.GetMany<CurrencyDto>("Currencies");
            if (currencies != null) {
                ViewData["CurrencyId"] = new SelectList(currencies, "CurrencyId", "Name");
            }
            return View();
        }

        // POST: VehicleMakes/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreationDto company)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Post(ApiUri, company);
                if (response == null)
                {
                    // log error
                }

                await HttpContext.Authentication.SignOutAsync("Cookies");
                await HttpContext.Authentication.SignOutAsync("oidc");
                return RedirectToAction("Index", "Home");
            }

            return View(company);
        }

        // GET: VehicleMakes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleMake = await _apiService.Get<CompanyUpdateDto>($"{ApiUri}/{id}");

            if (vehicleMake == null)
            {
                return NotFound();
            }

            ViewData["CompanyId"] = id;
            return View(vehicleMake);
        }

        // POST: VehicleMakes/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CompanyUpdateDto company)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Put($"{ApiUri}/{id}", company);
                if (response == null)
                {
                    // log error
                }

                return RedirectToAction("Details", new {id = id});
            }

            return View(company);
        }

        // GET: VehicleMakes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleMake = await _apiService.Get<CompanyDto>($"{ApiUri}/{id}");

            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _apiService.Delete<VehicleMakeDto>($"{ApiUri}/{id}");

            if (result == null)
            {
                // error
            }

            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");

            return RedirectToAction("Index", "Home");
        }
    }
}