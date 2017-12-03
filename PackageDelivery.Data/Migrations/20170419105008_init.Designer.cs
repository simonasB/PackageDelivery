using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.Data.Migrations
{
    [DbContext(typeof(PackageDeliveryContext))]
    [Migration("20170419105008_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfManufacture");

                    b.Property<bool>("IsActive");

                    b.Property<string>("PlateNumber");

                    b.Property<double>("TrunkLength");

                    b.Property<double>("TrunkVolume");

                    b.Property<double>("TrunkWidth");

                    b.Property<int>("VehicleModelId");

                    b.Property<int>("VehicleState");

                    b.Property<double>("Weight");

                    b.HasKey("VehicleId");

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

            modelBuilder.Entity("PackageDelivery.Domain.Entities.Vehicle", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PackageDelivery.Domain.Entities.VehicleModel", b =>
                {
                    b.HasOne("PackageDelivery.Domain.Entities.VehicleMake", "VehicleMake")
                        .WithMany("VehicleModels")
                        .HasForeignKey("VehicleMakeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
