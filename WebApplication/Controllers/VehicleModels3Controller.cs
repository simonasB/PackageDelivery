using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PackageDelivery.Domain.Entities;
using PackageDelivery.SharedKernel.Data;

namespace PackageDelivery.WebApplication.Controllers {
    [Authorize]
    //[Route("api/vehiclemakes/{vehicleMakeId}/vehiclemodels2")]
    public class VehicleModels3Controller : Controller {
        private readonly IGenericRepository<VehicleModel> _vehicleModelRepository;
        private readonly IGenericRepository<VehicleMake> _vehicleMakeRepository;
        private readonly ILogger<VehicleModels3Controller> _logger;

        public VehicleModels3Controller(IGenericRepository<VehicleModel> vehicleModelRepository,
            ILogger<VehicleModels3Controller> logger, IGenericRepository<VehicleMake> vehicleMakeRepository) {
            _vehicleModelRepository = vehicleModelRepository;
            _logger = logger;
            _vehicleMakeRepository = vehicleMakeRepository;
        }

        [HttpGet]
        public IActionResult GetVehicleModels(int vehicleMakeId)
        {
            var vehicleModels = _vehicleMakeRepository.FindByKeyInclude(vehicleMakeId, vehicleMake => vehicleMake.VehicleModels);
            return Ok(vehicleModels);
        }

        [HttpGet]
        //[Route("{id}", Name = "VehicleModelGet")]
        public IActionResult Get(int vehicleMakeId, int id) {
            var vehicleModel = _vehicleModelRepository.FindByKeyInclude(id, model => model.VehicleMake);
            if (vehicleModel == null) {
                return NotFound();
            }

            if (vehicleModel.VehicleMake.VehicleMakeId != vehicleMakeId) {
                return BadRequest();
            }

            return Ok(vehicleModel);
        }

        [HttpPost]
        public IActionResult Post(int vehicleMakeId, [FromBody] VehicleModel vehicleModel) {
            try {
                var vehicleMake = _vehicleMakeRepository.FindByKey(vehicleMakeId);
                if (vehicleMake == null) {
                    return BadRequest("Could not find Vehicle Make");
                }

                vehicleModel.VehicleMakeId = vehicleMake.VehicleMakeId;
                _vehicleModelRepository.Insert(vehicleModel);

                var url = Url.Link("VehicleModelGet",
                    new {vehicleMakeId = vehicleMakeId, id = vehicleModel.VehicleModelId});

                return Created(url, vehicleModel);
            } catch(Exception ex) {
                _logger.LogError($"Failed to save Vehicle Model for Vehicle Make {vehicleMakeId}: {ex}");
            }

            return BadRequest();
        }
    }
}
