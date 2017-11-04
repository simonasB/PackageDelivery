using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.Data {
    public class PackageDeliveryContext : IdentityDbContext<User> {
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = PackageDelivery; Trusted_Connection = True; ");
        }
    }
}
