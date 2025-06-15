using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class grbtany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId",
                table: "PostFavouriteLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId",
                table: "PostFavouriteLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId1",
                table: "PostFavouriteLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostFavouriteLists",
                table: "PostFavouriteLists");

            migrationBuilder.DropIndex(
                name: "IX_PostFavouriteLists_PostId",
                table: "PostFavouriteLists");

            migrationBuilder.DropIndex(
                name: "IX_PostFavouriteLists_PostId1",
                table: "PostFavouriteLists");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "PostFavouriteLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostFavouriteLists",
                table: "PostFavouriteLists",
                columns: new[] { "PostId", "FavouriteListId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_FavouriteListId",
                table: "PostFavouriteLists",
                column: "FavouriteListId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId",
                table: "PostFavouriteLists",
                column: "FavouriteListId",
                principalTable: "FavouriteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId",
                table: "PostFavouriteLists",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId",
                table: "PostFavouriteLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId",
                table: "PostFavouriteLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostFavouriteLists",
                table: "PostFavouriteLists");

            migrationBuilder.DropIndex(
                name: "IX_PostFavouriteLists_FavouriteListId",
                table: "PostFavouriteLists");

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "PostFavouriteLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostFavouriteLists",
                table: "PostFavouriteLists",
                columns: new[] { "FavouriteListId", "PostId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_PostId",
                table: "PostFavouriteLists",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_PostId1",
                table: "PostFavouriteLists",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId",
                table: "PostFavouriteLists",
                column: "FavouriteListId",
                principalTable: "FavouriteLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId",
                table: "PostFavouriteLists",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_Posts_PostId1",
                table: "PostFavouriteLists",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
