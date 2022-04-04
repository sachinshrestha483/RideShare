using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class AddTravelPrefrenceandSubTravelPrefrenceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TravelPrefrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPrefrences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubTravelPrefrences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TravelPrefrenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTravelPrefrences", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubTravelPrefrences_TravelPrefrences_TravelPrefrenceId",
                        column: x => x.TravelPrefrenceId,
                        principalTable: "TravelPrefrences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTravelPrefrences_TravelPrefrenceId",
                table: "SubTravelPrefrences",
                column: "TravelPrefrenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTravelPrefrences");

            migrationBuilder.DropTable(
                name: "TravelPrefrences");
        }
    }
}
