using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class MakeTableNamesPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Backlog_User_userId",
                table: "Backlog");

            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Backlog_backlogsid",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Developer_developersid",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genre_genresid",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platform_platformsid",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publisher_publisherId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Games_gameId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_User_authorId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platform",
                table: "Platform");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Developer",
                table: "Developer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Backlog",
                table: "Backlog");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Publisher",
                newName: "Publishers");

            migrationBuilder.RenameTable(
                name: "Platform",
                newName: "Platforms");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "Developer",
                newName: "Developers");

            migrationBuilder.RenameTable(
                name: "Backlog",
                newName: "Backlogs");

            migrationBuilder.RenameIndex(
                name: "IX_User_email",
                table: "Users",
                newName: "IX_Users_email");

            migrationBuilder.RenameIndex(
                name: "IX_Review_gameId",
                table: "Reviews",
                newName: "IX_Reviews_gameId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_authorId",
                table: "Reviews",
                newName: "IX_Reviews_authorId");

            migrationBuilder.RenameIndex(
                name: "IX_Backlog_userId",
                table: "Backlogs",
                newName: "IX_Backlogs_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Developers",
                table: "Developers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Backlogs",
                table: "Backlogs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Backlogs_backlogsid",
                table: "BacklogGame",
                column: "backlogsid",
                principalTable: "Backlogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Backlogs_Users_userId",
                table: "Backlogs",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Developers_developersid",
                table: "GameDeveloper",
                column: "developersid",
                principalTable: "Developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Genres_genresid",
                table: "GameGenre",
                column: "genresid",
                principalTable: "Genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platforms_platformsid",
                table: "GamePlatform",
                column: "platformsid",
                principalTable: "Platforms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_publisherId",
                table: "Games",
                column: "publisherId",
                principalTable: "Publishers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Games_gameId",
                table: "Reviews",
                column: "gameId",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_authorId",
                table: "Reviews",
                column: "authorId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Backlogs_backlogsid",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_Backlogs_Users_userId",
                table: "Backlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Developers_developersid",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genres_genresid",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platforms_platformsid",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_publisherId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Games_gameId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_authorId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publishers",
                table: "Publishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Developers",
                table: "Developers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Backlogs",
                table: "Backlogs");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Publishers",
                newName: "Publisher");

            migrationBuilder.RenameTable(
                name: "Platforms",
                newName: "Platform");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "Developers",
                newName: "Developer");

            migrationBuilder.RenameTable(
                name: "Backlogs",
                newName: "Backlog");

            migrationBuilder.RenameIndex(
                name: "IX_Users_email",
                table: "User",
                newName: "IX_User_email");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_gameId",
                table: "Review",
                newName: "IX_Review_gameId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_authorId",
                table: "Review",
                newName: "IX_Review_authorId");

            migrationBuilder.RenameIndex(
                name: "IX_Backlogs_userId",
                table: "Backlog",
                newName: "IX_Backlog_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publisher",
                table: "Publisher",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platform",
                table: "Platform",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Developer",
                table: "Developer",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Backlog",
                table: "Backlog",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Backlog_User_userId",
                table: "Backlog",
                column: "userId",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Backlog_backlogsid",
                table: "BacklogGame",
                column: "backlogsid",
                principalTable: "Backlog",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Developer_developersid",
                table: "GameDeveloper",
                column: "developersid",
                principalTable: "Developer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Genre_genresid",
                table: "GameGenre",
                column: "genresid",
                principalTable: "Genre",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platform_platformsid",
                table: "GamePlatform",
                column: "platformsid",
                principalTable: "Platform",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publisher_publisherId",
                table: "Games",
                column: "publisherId",
                principalTable: "Publisher",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Games_gameId",
                table: "Review",
                column: "gameId",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_User_authorId",
                table: "Review",
                column: "authorId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
