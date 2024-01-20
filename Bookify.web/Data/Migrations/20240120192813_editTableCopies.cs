using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.web.Data.Migrations
{
    public partial class editTableCopies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BookCopies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookCopies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "BookCopies",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "BookCopies");
        }
    }
}
