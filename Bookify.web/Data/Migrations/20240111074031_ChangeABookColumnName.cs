using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.web.Data.Migrations
{
    public partial class ChangeABookColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailableForRent",
                table: "Books",
                newName: "IsAvailableForRental");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailableForRental",
                table: "Books",
                newName: "IsAvailableForRent");
        }
    }
}
