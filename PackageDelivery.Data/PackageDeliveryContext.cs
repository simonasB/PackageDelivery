using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Data {
    public class PackageDeliveryContext : IdentityDbContext<User> {
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PickUpPoint> PickUpPoints { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //modelBuilder.Entity<Payment>()
            // .Property(o => o.Order).IsRequired();

            //modelBuilder.Entity<Order>()
            //.Property(o => o.Payment).IsRequired();

            /*foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }*/

            var entities = new List<string> {
                typeof(Address).FullName,
                typeof(Company).FullName,
                typeof(Country).FullName,
                typeof(Currency).FullName,
                typeof(Item).FullName,
                typeof(Order).FullName,
                typeof(Payment).FullName,
                typeof(PaymentMethod).FullName,
                typeof(PickUpPoint).FullName,
                typeof(Shipment).FullName,
                typeof(Vehicle).FullName,
                typeof(VehicleMake).FullName,
                typeof(VehicleModel).FullName
            };

            entities.ForEach(entity => {
                foreach (var relationship in modelBuilder.Model.FindEntityType(entity)
                    .GetForeignKeys()) {
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                }
            });

            foreach (var relationship in modelBuilder.Model.FindEntityType(typeof(User).FullName)
                .GetForeignKeys()) {
                relationship.DeleteBehavior = DeleteBehavior.SetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = PackageDelivery; Trusted_Connection = True; ");
        }
    }
}
