using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PackageDelivery.WebApplication.Data;
using PackageDelivery.Domain.Entities;

namespace PackageDelivery.WebApplication.Data.Migrations
{
    [DbContext(typeof(PackageDeliveryContext))]
    [Migration("20170522143741_VehicleCurrentVolumeAndWeight")]
    partial class VehicleCurrentVolumeAndWeight
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApartmentNumber");

                    b.Property<string>("City");

                    b.Property<int>("CountryId");

                    b.Property<string>("HouseNumber");

                    b.Property<string>("PostalCode");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("StreetName");

                    b.HasKey("AddressId");

                    b.HasIndex("CountryId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CubePrice");

                    b.Property<int>("CurrencyId");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<double>("KilogramPrice");

                    b.Property<double>("KilometerPrice");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<double>("VolumeAndWeightRatioToApplyVolumePrice");

                    b.HasKey("CompanyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("Height");

                    b.Property<double>("Length");

                    b.Property<int>("OrderId");

                    b.Property<double>("Volume");

                    b.Property<double>("Weight");

                    b.Property<double>("Width");

                    b.HasKey("ItemId");

                    b.HasIndex("OrderId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DeliveryAddressId");

                    b.Property<int?>("DeliveryShipmentId");

                    b.Property<double>("DistanceBetweenPickUpAndDeliveryAddresses");

                    b.Property<double>("DistanceBetweenPickUpPointAndDeliveryAddress");

                    b.Property<int>("OrderState");

                    b.Property<int>("PickUpAddressId");

                    b.Property<int>("PickUpPointId");

                    b.Property<int?>("PickUpShipmentId");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("DeliveryAddressId");

                    b.HasIndex("DeliveryShipmentId");

                    b.HasIndex("PickUpAddressId");

                    b.HasIndex("PickUpPointId");

                    b.HasIndex("PickUpShipmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderId");

                    b.Property<DateTime>("PaidDate");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("PaymentState");

                    b.Property<double>("Price");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.PickUpPoint", b =>
                {
                    b.Property<int>("PickUpPointId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<int>("CompanyId");

                    b.Property<bool>("IsStatic");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("PickUpPointId");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("CompanyId");

                    b.ToTable("PickUpPoints");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Shipment", b =>
                {
                    b.Property<int>("ShipmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("FinishDate");

                    b.Property<int>("ShipmentState");

                    b.Property<string>("UserId");

                    b.Property<int>("VehicleId");

                    b.HasKey("ShipmentId");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("BirthDate");

                    b.Property<int?>("CompanyId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("PickUpPointId");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("PickUpPointId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CarRegistrationDate");

                    b.Property<string>("Color");

                    b.Property<int>("CompanyId");

                    b.Property<double>("CurrentTrunkVolume");

                    b.Property<double>("CurrentTrunkWeight");

                    b.Property<DateTime>("DateOfManufacture");

                    b.Property<bool>("IsActive");

                    b.Property<double>("MaxTrunkWeight");

                    b.Property<int>("PickUpPointId");

                    b.Property<string>("PlateNumber");

                    b.Property<DateTime>("TechnicalReviewDate");

                    b.Property<double>("TrunkHeight");

                    b.Property<double>("TrunkLength");

                    b.Property<double>("TrunkVolume");

                    b.Property<double>("TrunkWidth");

                    b.Property<int>("VehicleModelId");

                    b.Property<int>("VehicleState");

                    b.Property<double>("Weight");

                    b.HasKey("VehicleId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PickUpPointId");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.VehicleMake", b =>
                {
                    b.Property<int>("VehicleMakeId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("VehicleMakeId");

                    b.ToTable("VehicleMakes");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.VehicleModel", b =>
                {
                    b.Property<int>("VehicleModelId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("VehicleMakeId");

                    b.HasKey("VehicleModelId");

                    b.HasIndex("VehicleMakeId");

                    b.ToTable("VehicleModels");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.HasOne("PackageDelivery.Domain.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Address", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Country", "Country")
                        .WithMany("Addresses")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Company", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Currency", "Currency")
                        .WithMany("Companies")
                        .HasForeignKey("CurrencyId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Item", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Order", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Address", "DeliveryAddress")
                        .WithMany("DeliveryOrders")
                        .HasForeignKey("DeliveryAddressId");

                    b.HasOne("PackageDelivery.Domain.Entities.Shipment", "DeliveryShipment")
                        .WithMany("DeliveryOrders")
                        .HasForeignKey("DeliveryShipmentId");

                    b.HasOne("PackageDelivery.Domain.Entities.Address", "PickUpAddress")
                        .WithMany("PickUpOrders")
                        .HasForeignKey("PickUpAddressId");

                    b.HasOne("PackageDelivery.Domain.Entities.PickUpPoint", "PickUpPoint")
                        .WithMany("Orders")
                        .HasForeignKey("PickUpPointId");

                    b.HasOne("PackageDelivery.Domain.Entities.Shipment", "PickUpShipment")
                        .WithMany("PickUpOrders")
                        .HasForeignKey("PickUpShipmentId");

                    b.HasOne("PackageDelivery.Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Payment", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("PackageDelivery.Domain.Entities.Payment", "OrderId");

                    b.HasOne("PackageDelivery.Domain.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentMethodId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.PickUpPoint", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Address", "Address")
                        .WithOne("PickUpPoint")
                        .HasForeignKey("PackageDelivery.Domain.Entities.PickUpPoint", "AddressId");

                    b.HasOne("PackageDelivery.Domain.Entities.Company", "Company")
                        .WithMany("PickUpPoints")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Shipment", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.User", "User")
                        .WithMany("Shipments")
                        .HasForeignKey("UserId");

                    b.HasOne("PackageDelivery.Domain.Entities.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.User", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("PackageDelivery.Domain.Entities.PickUpPoint", "PickUpPoint")
                        .WithMany("Users")
                        .HasForeignKey("PickUpPointId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.Company", "Company")
                        .WithMany("Vehicles")
                        .HasForeignKey("CompanyId");

                    b.HasOne("PackageDelivery.Domain.Entities.PickUpPoint", "PickUpPoint")
                        .WithMany("Vehicles")
                        .HasForeignKey("PickUpPointId");

                    b.HasOne("PackageDelivery.Domain.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleModelId");
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.VehicleModel", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.VehicleMake", "VehicleMake")
                        .WithMany("VehicleModels")
                        .HasForeignKey("VehicleMakeId");
                });
        }
    }
}
