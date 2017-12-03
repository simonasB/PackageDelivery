using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.Data.Migrations
{
    public partial class DeliveryAndPickupShipmentRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipments_ShipmentId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "Orders",
                newName: "PickUpShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShipmentId",
                table: "Orders",
                newName: "IX_Orders_PickUpShipmentId");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryShipmentId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryShipmentId",
                table: "Orders",
                column: "DeliveryShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipments_DeliveryShipmentId",
                table: "Orders",
                column: "DeliveryShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipments_PickUpShipmentId",
                table: "Orders",
                column: "PickUpShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipments_DeliveryShipmentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipments_PickUpShipmentId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryShipmentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryShipmentId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PickUpShipmentId",
                table: "Orders",
                newName: "ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PickUpShipmentId",
                table: "Orders",
                newName: "IX_Orders_ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipments_ShipmentId",
                table: "Orders",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
