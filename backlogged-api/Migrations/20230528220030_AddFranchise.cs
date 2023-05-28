using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class AddFranchise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "franchiseId",
                table: "Games",
                type: "uuid",
                nullable: true,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_franchiseId",
                table: "Games",
                column: "franchiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Franchises_franchiseId",
                table: "Games",
                column: "franchiseId",
                principalTable: "Franchises",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Franchises_franchiseId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Franchises");

            migrationBuilder.DropIndex(
                name: "IX_Games_franchiseId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "franchiseId",
                table: "Games");
        }
    }
}
