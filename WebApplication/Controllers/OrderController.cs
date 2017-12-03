using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PackageDelivery.Data;
using PackageDelivery.Services.Maps;
using PackageDelivery.WebApplication.Models.OrderViewModel;

namespace PackageDelivery.WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly PackageDeliveryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDistanceCalculator _orderDistanceCalculator;
        private readonly int _selectedCompanyId;

        public OrderController(PackageDeliveryContext context, UserManager<User> userManager, IDistanceCalculator distanceCalculator)
        {
            _context = context;
            _userManager = userManager;
            _orderDistanceCalculator = distanceCalculator;
            _selectedCompanyId =  _context.Companies.First().CompanyId;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var packageDeliveryContext = _context.Orders.Include(o => o.DeliveryAddress).Include(o => o.DeliveryShipment).Include(o => o.PickUpAddress).Include(o => o.PickUpPoint).Include(o => o.PickUpShipment).Include(o => o.User);
            return View(await packageDeliveryContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.DeliveryShipment)
                .Include(o => o.PickUpAddress)
                .Include(o => o.PickUpPoint)
                .Include(o => o.PickUpShipment)
                .Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/ShowOrderRegistration
        public IActionResult ShowOrderRegistration()
        {
            var orderRegistrationViewModel = new OrderRegistrationViewModel();
            ViewData["DeliveryAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["DeliveryShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId");
            ViewData["PickUpAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["PickUpPointId"] = new SelectList(_context.PickUpPoints, "PickUpPointId", "PickUpPointId");
            ViewData["PickUpShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PickUpCountry"] = new SelectList(_context.Countries, "CountryId", "Name");
            ViewData["DeliveryCountry"] = new SelectList(_context.Countries, "CountryId", "Name");
            orderRegistrationViewModel.isCompanySelected = (_selectedCompanyId > -1);
            return View("OrderRegistration", orderRegistrationViewModel);
        }

        // POST: Order/RegisterOrder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOrder(OrderRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = _context.Companies.Where(o => o.CompanyId == _selectedCompanyId).FirstOrDefault();
                // PickupAddress
                var pickUpAddress = new Address();
                pickUpAddress.StreetName = model.PickUpStreet + " " + model.PickUpStreetNr.ToString();
                pickUpAddress.HouseNumber = model.PickUpHouseNr.ToString();
                pickUpAddress.PostalCode = model.PickUpZipCode.ToString();
                pickUpAddress.CountryId = model.PickUpCountry;
                pickUpAddress.City = model.PickUpCity;
                _context.Add(pickUpAddress);
                _context.SaveChanges();

                // DeliveryAddress
                var deliveryAddress = new Address();
                deliveryAddress.StreetName = model.DeliveryStreet + " " + model.DeliveryStreetNr.ToString();
                deliveryAddress.HouseNumber = model.DeliveryHouseNr.ToString();
                deliveryAddress.PostalCode = model.DeliveryZipCode.ToString();
                deliveryAddress.CountryId = model.DeliveryCountry;
                deliveryAddress.City = model.DeliveryCity;
                _context.Add(deliveryAddress);
                _context.SaveChanges();
                
                /**
                 * 
                 foreach(Item i in model.items)
                 {
                     _context.Add(i);
                     _context.SaveChanges();
                 }
                 **/
                // OrderItem
                var item = new Item();
                item.Length = model.OrderItemLength;
                item.Width = model.OrderItemWidth;
                item.Height = model.OrderItemHeight;
                item.Weight = model.OrderItemWeight;
                var httpClient = new HttpClient();
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user != null)
                {
                    // Order
                    model.order = new Order();
                    model.order.DeliveryAddressId = deliveryAddress.AddressId;
                    model.order.PickUpAddressId = pickUpAddress.AddressId;
                    model.order.DeliveryAddress = deliveryAddress;
                    model.order.DistanceBetweenPickUpAndDeliveryAddresses = _orderDistanceCalculator.CalculateDistanceBetweenTwoPoints(pickUpAddress, deliveryAddress);
                    model.order.PickUpAddress = pickUpAddress;
                    model.order.User = user;
                    var itemsList = new List<Item>();
                    itemsList.Add(item);
                    model.order.Items = itemsList;
                    model.order.Items.Add(item);
                    _context.Add(model.order);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "PaymentMethod");
                } else
                {
                    return View("OrderRegistration", model);
                }
            }
            return View("OrderRegistration", model);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["DeliveryAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", order.DeliveryAddressId);
            ViewData["DeliveryShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", order.DeliveryShipmentId);
            ViewData["PickUpAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", order.PickUpAddressId);
            ViewData["PickUpPointId"] = new SelectList(_context.PickUpPoints, "PickUpPointId", "PickUpPointId", order.PickUpPointId);
            ViewData["PickUpShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", order.PickUpShipmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,RegistrationDate,OrderState,PickUpAddressId,DeliveryAddressId,PickUpPointId,UserId,PickUpShipmentId,DeliveryShipmentId,DistanceBetweenPickUpAndDeliveryAddresses,DistanceBetweenPickUpPointAndDeliveryAddress,Weight,Volume,Length,Width,Height")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DeliveryAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", order.DeliveryAddressId);
            ViewData["DeliveryShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", order.DeliveryShipmentId);
            ViewData["PickUpAddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", order.PickUpAddressId);
            ViewData["PickUpPointId"] = new SelectList(_context.PickUpPoints, "PickUpPointId", "PickUpPointId", order.PickUpPointId);
            ViewData["PickUpShipmentId"] = new SelectList(_context.Shipments, "ShipmentId", "ShipmentId", order.PickUpShipmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.DeliveryAddress)
                .Include(o => o.DeliveryShipment)
                .Include(o => o.PickUpAddress)
                .Include(o => o.PickUpPoint)
                .Include(o => o.PickUpShipment)
                .Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
