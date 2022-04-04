using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class UpdatedTheVehicleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vehicle",
                newName: "ModelName");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "Vehicle",
                newName: "Name");
        }
    }
}
