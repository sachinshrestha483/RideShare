using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class AddedShowinTravelAndSubtrravelPrefrences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "show",
                table: "TravelPrefrences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show",
                table: "SubTravelPrefrences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "show",
                table: "TravelPrefrences");

            migrationBuilder.DropColumn(
                name: "show",
                table: "SubTravelPrefrences");
        }
    }
}
