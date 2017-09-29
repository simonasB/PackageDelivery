using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;
using PackageDelivery.WebApplication.Data;

namespace PackageDelivery.WebApplication.Models.OrderViewModel
{
    public class OrderRegistrationViewModel
    {
        public Order order { get; set; }
        public double OrderItemLength { get; set; }
        public double OrderItemWidth { get; set; }
        public double OrderItemHeight { get; set; }
        public double OrderItemWeight { get; set; }
        public int PickUpCountry { get; set; }
        public int DeliveryCountry { get; set; }

        public String PickUpStreet { get; set; }
        public String PickUpCity { get; set; }
        public int PickUpStreetNr { get; set; }
        public int PickUpHouseNr { get; set; }
        public int PickUpZipCode { get; set; }

        public String DeliveryStreet { get; set; }
        public String DeliveryCity { get; set; }
        public int DeliveryStreetNr { get; set; }
        public int DeliveryHouseNr { get; set; }
        public int DeliveryZipCode { get; set; }

        public IEnumerable<Item> items { get; set; }

        public bool isCompanySelected { get; set; }
    }
}
