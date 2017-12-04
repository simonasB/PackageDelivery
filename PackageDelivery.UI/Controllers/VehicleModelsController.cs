using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageDelivery.Domain.Dtos.VehicleMakeDtos;
using PackageDelivery.UI.Api;

namespace PackageDelivery.UI.Controllers
{
    [Authorize]
    public class VehicleModelsController : Controller
    {
        private readonly IApiService _apiService;
        private const string ApiUri = "VehicleModels";

        public VehicleModelsController(IApiService apiService) {
            _apiService = apiService;
        }

        // GET: VehicleMakes
        public async Task<IActionResult> Index(int vehicleMakeId)
        {
            return View(await _apiService.GetMany<VehicleMakeDto>(GetApiUri(vehicleMakeId)));
        }

        // GET: VehicleMakes/Details/5
        public async Task<IActionResult> Details(int id) {
            var vehicleMake = await _apiService.Get<VehicleMakeDto>($"{ApiUri}/{id}");

            if (vehicleMake == null) {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // GET: VehicleMakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMakes/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleMakeCreationDto vehicleMake)
        {
            if (ModelState.IsValid) {
                var response = await _apiService.Post(ApiUri, vehicleMake);
                if (response == null) {
                    // log error
                }

                return RedirectToAction("Index");
            }

            return View(vehicleMake);
        }

        // GET: VehicleMakes/Edit/5
        public async Task<IActionResult> Edit(int id) {
            var vehicleMake = await _apiService.Get<VehicleMakeUpdateDto>($"{ApiUri}/{id}");

            if (vehicleMake == null) {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // POST: VehicleMakes/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, VehicleMakeUpdateDto vehicleMake) {
            if (ModelState.IsValid) {
                var response = await _apiService.Put($"{ApiUri}/{id}", vehicleMake);
                if (response == null) {
                    // log error
                }

                return RedirectToAction("Index");
            }

            return View(vehicleMake);
        }

        // GET: VehicleMakes/Delete/5
        public async Task<IActionResult> Delete(int id) {
            var vehicleMake = await _apiService.Get<VehicleMakeDto>($"{ApiUri}/{id}");

            if (vehicleMake == null) {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _apiService.Delete<VehicleMakeDto>($"{ApiUri}/{id}");

            if (result == null) {
                // error
            }

            return RedirectToAction("Index");
        }

        private string GetApiUri(int vehicleMakeId) {
            return $"VehicleModels/{vehicleMakeId}";
        }
    }
}