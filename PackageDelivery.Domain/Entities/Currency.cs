using System.Collections.Generic;

namespace PackageDelivery.Domain.Entities {
    public class Currency {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
