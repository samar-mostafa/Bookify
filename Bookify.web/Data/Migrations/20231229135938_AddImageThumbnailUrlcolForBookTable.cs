﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.web.Data.Migrations
{
    public partial class AddImageThumbnailUrlcolForBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnailUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumbnailUrl",
                table: "Books");
        }
    }
}
