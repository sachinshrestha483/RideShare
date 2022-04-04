using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class cvdjgterhtr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Ride",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ride_VehicleId",
                table: "Ride",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ride_Vehicle_VehicleId",
                table: "Ride",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ride_Vehicle_VehicleId",
                table: "Ride");

            migrationBuilder.DropIndex(
                name: "IX_Ride_VehicleId",
                table: "Ride");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Ride");
        }
    }
}
