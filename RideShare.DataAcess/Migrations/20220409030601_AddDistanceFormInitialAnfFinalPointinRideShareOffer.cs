using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class AddDistanceFormInitialAnfFinalPointinRideShareOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistancefromFinalLocation",
                table: "RideShareOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistancefromInitialLocation",
                table: "RideShareOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistancefromFinalLocation",
                table: "RideShareOffers");

            migrationBuilder.DropColumn(
                name: "DistancefromInitialLocation",
                table: "RideShareOffers");
        }
    }
}
