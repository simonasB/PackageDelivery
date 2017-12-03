using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.Data.Migrations
{
    public partial class PickupPointAddressNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "PickUpPoints",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "PickUpPoints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullAddress",
                table: "PickUpPoints");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "PickUpPoints",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
