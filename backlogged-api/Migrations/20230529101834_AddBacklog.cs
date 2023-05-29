using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class AddBacklog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "backlogId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "authorId",
                table: "Review",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Backlog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    userId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    isVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backlog", x => x.id);
                    table.ForeignKey(
                        name: "FK_Backlog_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BacklogGame",
                columns: table => new
                {
                    backlogsid = table.Column<Guid>(type: "uuid", nullable: false),
                    gamesid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogGame", x => new { x.backlogsid, x.gamesid });
                    table.ForeignKey(
                        name: "FK_BacklogGame_Backlog_backlogsid",
                        column: x => x.backlogsid,
                        principalTable: "Backlog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BacklogGame_Games_gamesid",
                        column: x => x.gamesid,
                        principalTable: "Games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Backlog_userId",
                table: "Backlog",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BacklogGame_gamesid",
                table: "BacklogGame",
                column: "gamesid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BacklogGame");

            migrationBuilder.DropTable(
                name: "Backlog");

            migrationBuilder.DropColumn(
                name: "backlogId",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "authorId",
                table: "Review",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
