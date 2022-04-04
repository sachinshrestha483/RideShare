using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class AddUserTravellPrefrences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTravellPrefrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubTravelPrefrenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTravellPrefrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTravellPrefrences_SubTravelPrefrences_SubTravelPrefrenceId",
                        column: x => x.SubTravelPrefrenceId,
                        principalTable: "SubTravelPrefrences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTravellPrefrences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTravellPrefrences_SubTravelPrefrenceId",
                table: "UserTravellPrefrences",
                column: "SubTravelPrefrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTravellPrefrences_UserId",
                table: "UserTravellPrefrences",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTravellPrefrences");
        }
    }
}
