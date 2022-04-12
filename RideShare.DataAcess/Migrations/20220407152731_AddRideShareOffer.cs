using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class AddRideShareOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideShareOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: false),
                    StartLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLocationLatitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    StartLocationLongitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    EndLocationLatitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    EndLocationLongitude = table.Column<decimal>(type: "decimal(12,9)", nullable: false),
                    OfferedPrice = table.Column<int>(type: "int", nullable: false),
                    NotesForRideCreater = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesForOfferCreator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RideShareOfferStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideShareOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideShareOffers_Ride_RideId",
                        column: x => x.RideId,
                        principalTable: "Ride",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideShareOffers_RideId",
                table: "RideShareOffers",
                column: "RideId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideShareOffers");
        }
    }
}
