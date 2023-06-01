using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backlogged_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropertyCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Backlogs_backlogsid",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Games_gamesid",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_Backlogs_AspNetUsers_userId",
                table: "Backlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Developers_developersid",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Games_gamesid",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Games_gamesid",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genres_genresid",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Games_gamesid",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platforms_platformsid",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Franchises_franchiseId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_publisherId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_authorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Games_gameId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "registeredAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Reviews",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "gameId",
                table: "Reviews",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "details",
                table: "Reviews",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Reviews",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "authorId",
                table: "Reviews",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_gameId",
                table: "Reviews",
                newName: "IX_Reviews_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_authorId",
                table: "Reviews",
                newName: "IX_Reviews_AuthorId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Publishers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Publishers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Platforms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Platforms",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Games",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "releaseDate",
                table: "Games",
                newName: "ReleaseDate");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Games",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "publisherId",
                table: "Games",
                newName: "PublisherId");

            migrationBuilder.RenameColumn(
                name: "franchiseId",
                table: "Games",
                newName: "FranchiseId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Games",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "coverImageUrl",
                table: "Games",
                newName: "CoverImageUrl");

            migrationBuilder.RenameColumn(
                name: "backgroundImageUrl",
                table: "Games",
                newName: "BackgroundImageUrl");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Games",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Games_publisherId",
                table: "Games",
                newName: "IX_Games_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_franchiseId",
                table: "Games",
                newName: "IX_Games_FranchiseId");

            migrationBuilder.RenameColumn(
                name: "platformsid",
                table: "GamePlatform",
                newName: "PlatformsId");

            migrationBuilder.RenameColumn(
                name: "gamesid",
                table: "GamePlatform",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_platformsid",
                table: "GamePlatform",
                newName: "IX_GamePlatform_PlatformsId");

            migrationBuilder.RenameColumn(
                name: "genresid",
                table: "GameGenre",
                newName: "GenresId");

            migrationBuilder.RenameColumn(
                name: "gamesid",
                table: "GameGenre",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameGenre_genresid",
                table: "GameGenre",
                newName: "IX_GameGenre_GenresId");

            migrationBuilder.RenameColumn(
                name: "gamesid",
                table: "GameDeveloper",
                newName: "GamesId");

            migrationBuilder.RenameColumn(
                name: "developersid",
                table: "GameDeveloper",
                newName: "DevelopersId");

            migrationBuilder.RenameIndex(
                name: "IX_GameDeveloper_gamesid",
                table: "GameDeveloper",
                newName: "IX_GameDeveloper_GamesId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Franchises",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Franchises",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Developers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Developers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Backlogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "isVisible",
                table: "Backlogs",
                newName: "IsVisible");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Backlogs",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Backlogs_userId",
                table: "Backlogs",
                newName: "IX_Backlogs_UserId");

            migrationBuilder.RenameColumn(
                name: "gamesid",
                table: "BacklogGame",
                newName: "GamesId");

            migrationBuilder.RenameColumn(
                name: "backlogsid",
                table: "BacklogGame",
                newName: "BacklogsId");

            migrationBuilder.RenameIndex(
                name: "IX_BacklogGame_gamesid",
                table: "BacklogGame",
                newName: "IX_BacklogGame_GamesId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "AspNetUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "profileImageUrl",
                table: "AspNetUsers",
                newName: "ProfileImageUrl");

            migrationBuilder.RenameColumn(
                name: "passwordHash",
                table: "AspNetUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AspNetUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "backlogId",
                table: "AspNetUsers",
                newName: "BacklogId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Email");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Backlogs_BacklogsId",
                table: "BacklogGame",
                column: "BacklogsId",
                principalTable: "Backlogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Games_GamesId",
                table: "BacklogGame",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Backlogs_AspNetUsers_UserId",
                table: "Backlogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Developers_DevelopersId",
                table: "GameDeveloper",
                column: "DevelopersId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Games_GamesId",
                table: "GameDeveloper",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Games_GamesId",
                table: "GameGenre",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Genres_GenresId",
                table: "GameGenre",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Games_GamesId",
                table: "GamePlatform",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platforms_PlatformsId",
                table: "GamePlatform",
                column: "PlatformsId",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Franchises_FranchiseId",
                table: "Games",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_PublisherId",
                table: "Games",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Games_GameId",
                table: "Reviews",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Backlogs_BacklogsId",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_BacklogGame_Games_GamesId",
                table: "BacklogGame");

            migrationBuilder.DropForeignKey(
                name: "FK_Backlogs_AspNetUsers_UserId",
                table: "Backlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Developers_DevelopersId",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameDeveloper_Games_GamesId",
                table: "GameDeveloper");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Games_GamesId",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genres_GenresId",
                table: "GameGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Games_GamesId",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platforms_PlatformsId",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Franchises_FranchiseId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_PublisherId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Games_GameId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Reviews",
                newName: "gameId");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Reviews",
                newName: "details");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reviews",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Reviews",
                newName: "authorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_GameId",
                table: "Reviews",
                newName: "IX_Reviews_gameId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                newName: "IX_Reviews_authorId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Publishers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Publishers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Platforms",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Platforms",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Games",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Games",
                newName: "releaseDate");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Games",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "PublisherId",
                table: "Games",
                newName: "publisherId");

            migrationBuilder.RenameColumn(
                name: "FranchiseId",
                table: "Games",
                newName: "franchiseId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Games",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Games",
                newName: "coverImageUrl");

            migrationBuilder.RenameColumn(
                name: "BackgroundImageUrl",
                table: "Games",
                newName: "backgroundImageUrl");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Games",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Games_PublisherId",
                table: "Games",
                newName: "IX_Games_publisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_FranchiseId",
                table: "Games",
                newName: "IX_Games_franchiseId");

            migrationBuilder.RenameColumn(
                name: "PlatformsId",
                table: "GamePlatform",
                newName: "platformsid");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GamePlatform",
                newName: "gamesid");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_PlatformsId",
                table: "GamePlatform",
                newName: "IX_GamePlatform_platformsid");

            migrationBuilder.RenameColumn(
                name: "GenresId",
                table: "GameGenre",
                newName: "genresid");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameGenre",
                newName: "gamesid");

            migrationBuilder.RenameIndex(
                name: "IX_GameGenre_GenresId",
                table: "GameGenre",
                newName: "IX_GameGenre_genresid");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameDeveloper",
                newName: "gamesid");

            migrationBuilder.RenameColumn(
                name: "DevelopersId",
                table: "GameDeveloper",
                newName: "developersid");

            migrationBuilder.RenameIndex(
                name: "IX_GameDeveloper_GamesId",
                table: "GameDeveloper",
                newName: "IX_GameDeveloper_gamesid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Franchises",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Franchises",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Developers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Developers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Backlogs",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Backlogs",
                newName: "isVisible");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Backlogs",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Backlogs_UserId",
                table: "Backlogs",
                newName: "IX_Backlogs_userId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "BacklogGame",
                newName: "gamesid");

            migrationBuilder.RenameColumn(
                name: "BacklogsId",
                table: "BacklogGame",
                newName: "backlogsid");

            migrationBuilder.RenameIndex(
                name: "IX_BacklogGame_GamesId",
                table: "BacklogGame",
                newName: "IX_BacklogGame_gamesid");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AspNetUsers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                newName: "profileImageUrl");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "AspNetUsers",
                newName: "passwordHash");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "BacklogId",
                table: "AspNetUsers",
                newName: "backlogId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_email");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "registeredAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Backlogs_backlogsid",
                table: "BacklogGame",
                column: "backlogsid",
                principalTable: "Backlogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogGame_Games_gamesid",
                table: "BacklogGame",
                column: "gamesid",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Backlogs_AspNetUsers_userId",
                table: "Backlogs",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Developers_developersid",
                table: "GameDeveloper",
                column: "developersid",
                principalTable: "Developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeveloper_Games_gamesid",
                table: "GameDeveloper",
                column: "gamesid",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Games_gamesid",
                table: "GameGenre",
                column: "gamesid",
                principalTable: "Games",
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
                name: "FK_GamePlatform_Games_gamesid",
                table: "GamePlatform",
                column: "gamesid",
                principalTable: "Games",
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
                name: "FK_Games_Franchises_franchiseId",
                table: "Games",
                column: "franchiseId",
                principalTable: "Franchises",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_publisherId",
                table: "Games",
                column: "publisherId",
                principalTable: "Publishers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_authorId",
                table: "Reviews",
                column: "authorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Games_gameId",
                table: "Reviews",
                column: "gameId",
                principalTable: "Games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
