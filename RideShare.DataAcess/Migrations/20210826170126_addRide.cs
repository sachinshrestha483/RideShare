using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class addRide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ride",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartLocationLatitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartLocationLongitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EndLocationLatitude = table.Column<double>(type: "float", nullable: false),
                    EndLocationLongitude = table.Column<double>(type: "float", nullable: false),
                    DateTimeOfRide = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ride", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ride");
        }
    }
}
