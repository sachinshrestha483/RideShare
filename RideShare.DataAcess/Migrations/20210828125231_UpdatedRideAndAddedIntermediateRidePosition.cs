using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class UpdatedRideAndAddedIntermediateRidePosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteName",
                table: "Ride",
                newName: "RouteVia");

            migrationBuilder.AddColumn<float>(
                name: "DistanceinMeter",
                table: "Ride",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "RideIntermediatePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionLatitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PositionLongitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideIntermediatePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideIntermediatePositions_Ride_RideId",
                        column: x => x.RideId,
                        principalTable: "Ride",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideIntermediatePositions_RideId",
                table: "RideIntermediatePositions",
                column: "RideId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideIntermediatePositions");

            migrationBuilder.DropColumn(
                name: "DistanceinMeter",
                table: "Ride");

            migrationBuilder.RenameColumn(
                name: "RouteVia",
                table: "Ride",
                newName: "RouteName");
        }
    }
}
