using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDeveloper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Developer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GameDeveloper",
                columns: table => new
                {
                    developersid = table.Column<Guid>(type: "uuid", nullable: false),
                    gamesid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeveloper", x => new { x.developersid, x.gamesid });
                    table.ForeignKey(
                        name: "FK_GameDeveloper_Developer_developersid",
                        column: x => x.developersid,
                        principalTable: "Developer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDeveloper_Games_gamesid",
                        column: x => x.gamesid,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameDeveloper_gamesid",
                table: "GameDeveloper",
                column: "gamesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDeveloper");

            migrationBuilder.DropTable(
                name: "Developer");
        }
    }
}
