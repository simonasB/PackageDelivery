using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.Data.Migrations
{
    public partial class VehicleCurrentVolumeAndWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentTrunkVolume",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentTrunkWeight",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxTrunkWeight",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentTrunkVolume",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CurrentTrunkWeight",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MaxTrunkWeight",
                table: "Vehicles");
        }
    }
}
