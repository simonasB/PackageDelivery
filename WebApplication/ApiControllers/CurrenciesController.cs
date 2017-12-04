using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageDelivery.Data;
using PackageDelivery.Domain.Dtos.CurrencyDtos;
using PackageDelivery.WebApplication.Filters;

namespace PackageDelivery.WebApplication.ApiControllers {
    [Route("api/Currencies")]
    [ValidateModel]
    public class CurrenciesController : Controller {
        private readonly PackageDeliveryContext _context;
        private readonly IMapper _mapper;

        public CurrenciesController(PackageDeliveryContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCurrencies() {
            return Ok(_mapper.Map<IEnumerable<CurrencyDto>>(_context.Currencies));
        }
    }
}
