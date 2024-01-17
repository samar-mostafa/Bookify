﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.web.Data.Migrations
{
    public partial class AddBookCopiesTableAndSerialNumberSequance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Shared");

            migrationBuilder.CreateSequence<int>(
                name: "SerialNumber",
                schema: "Shared",
                startValue: 1000001L);

            migrationBuilder.CreateTable(
                name: "BookCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
                    SerialNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR Shared.SerialNumber")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCopies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopies",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCopies");

            migrationBuilder.DropSequence(
                name: "SerialNumber",
                schema: "Shared");
        }
    }
}
