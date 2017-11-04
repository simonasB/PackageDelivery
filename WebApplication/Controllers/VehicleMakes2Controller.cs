using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackageDelivery.Domain.Entities;
using PackageDelivery.SharedKernel.Data;

namespace PackageDelivery.WebApplication.Controllers {
    [Authorize]
    //[Route("api/[controller]2")]
    public class VehicleMakes2Controller : Controller {
        private readonly IGenericRepository<VehicleMake> _repository;
        private readonly ILogger<VehicleMakes2Controller> _logger;

        public VehicleMakes2Controller(IGenericRepository<VehicleMake> repository, ILogger<VehicleMakes2Controller> logger) {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get() {
            var vehicleMakes = _repository.All();
            return Ok(vehicleMakes);
        }

        [HttpGet]
        //[Route("{id}", Name = "VehicleMakeGet")]
        public IActionResult Get(int id) {
            var vehicleMake = _repository.FindByKeyInclude(id, x => x.VehicleModels);
            if (vehicleMake == null) {
                return NotFound($"VehicleMake {id} was not found.");
            }

            return Ok(vehicleMake);
        }

        [HttpPost]
        public IActionResult Post([FromBody] VehicleMake vehicleMake) {
            try {
                _logger.LogInformation("Creating a new Vehicle Make");
                _repository.Insert(vehicleMake);
                var newUri = Url.Link("VehicleMakeGet", new {id = vehicleMake.VehicleMakeId});
                return Created(newUri, vehicleMake);
            } catch (Exception ex) {
                _logger.LogError($"Exception was thrown while saving Vehicle Make: {ex}");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] VehicleMake vehicleMake) {
            try {
                var oldVehicleMake = _repository.FindByKey(id);
                if (oldVehicleMake == null) {
                    return NotFound($"Could not find a camp with and ID of {id}");
                }

                oldVehicleMake.Name = vehicleMake.Name ?? oldVehicleMake.Name;
                oldVehicleMake.IsActive = vehicleMake.IsActive;
                oldVehicleMake.VehicleModels = vehicleMake.VehicleModels ?? oldVehicleMake.VehicleModels;

                _repository.Insert(oldVehicleMake);
            } catch {
                
            }

            return BadRequest();
        }

        //[HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            try {
                var vehicleMake = _repository.FindByKey(id);
                if (vehicleMake == null) {
                    return NotFound($"Could not find Vehicle Make with ID of {id}");
                }

                _repository.Delete(id);

                return Ok();
            } catch {
                
            }

            return BadRequest("Could net delete Vehicle Make");
        }
    }
}
