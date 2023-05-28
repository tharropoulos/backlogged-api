using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "publisherId",
                table: "Games",
                type: "uuid",
                nullable: true,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_publisherId",
                table: "Games",
                column: "publisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publisher_publisherId",
                table: "Games",
                column: "publisherId",
                principalTable: "Publisher",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publisher_publisherId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropIndex(
                name: "IX_Games_publisherId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "publisherId",
                table: "Games");
        }
    }
}
