using System;

namespace PackageDelivery.Domain.Entities {
    public class Payment {
        public int PaymentId { get; set; }
        public PaymentState PaymentState { get; set; }
        public DateTime PaidDate { get; set; }
        public double Price { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod{ get; set; }
        public int PaymentMethodId { get; set; }
    }
}
