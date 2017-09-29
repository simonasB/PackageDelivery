using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PackageDelivery.WebApplication.Data.Migrations
{
    public partial class OrderDistances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DistanceBetweenPickUpAndDeliveryAddresses",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceBetweenPickUpPointAndDeliveryAddress",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceBetweenPickUpAndDeliveryAddresses",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DistanceBetweenPickUpPointAndDeliveryAddress",
                table: "Orders");
        }
    }
}
