using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DataAcess.Migrations
{
    public partial class UpdatedUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserProfilePhotoLink",
                table: "Users",
                newName: "UserProfilePhotoPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserProfilePhotoPath",
                table: "Users",
                newName: "UserProfilePhotoLink");
        }
    }
}
