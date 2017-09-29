﻿namespace PackageDelivery.Domain.Entities {
    public class Item {
        public int ItemId { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Volume { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}
