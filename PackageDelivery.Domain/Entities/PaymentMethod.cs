using System;
using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class PaymentMethod {
        public int PaymentMethodId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
