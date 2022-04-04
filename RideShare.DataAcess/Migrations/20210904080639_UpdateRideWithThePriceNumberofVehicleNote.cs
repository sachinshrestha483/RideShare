using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class UpdateRideWithThePriceNumberofVehicleNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Ride",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberofPassenger",
                table: "Ride",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Ride",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

         
         
         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
         
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Ride");

            migrationBuilder.DropColumn(
                name: "NumberofPassenger",
                table: "Ride");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Ride");

                 }
    }
}
