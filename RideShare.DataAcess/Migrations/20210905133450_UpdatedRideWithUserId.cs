using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class UpdatedRideWithUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ride",
                type: "int",
                nullable: false,
                defaultValue: 0);

     
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
     

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ride");
        }
    }
}
