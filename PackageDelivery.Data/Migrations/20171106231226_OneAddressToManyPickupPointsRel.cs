using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.Data.Migrations
{
    public partial class OneAddressToManyPickupPointsRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PickUpPoints_AddressId",
                table: "PickUpPoints");

            migrationBuilder.CreateIndex(
                name: "IX_PickUpPoints_AddressId",
                table: "PickUpPoints",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PickUpPoints_AddressId",
                table: "PickUpPoints");

            migrationBuilder.CreateIndex(
                name: "IX_PickUpPoints_AddressId",
                table: "PickUpPoints",
                column: "AddressId",
                unique: true);
        }
    }
}
