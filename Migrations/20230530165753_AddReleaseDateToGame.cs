using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseDateToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "releaseDate",
                table: "Games",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "releaseDate",
                table: "Games");
        }
    }
}
