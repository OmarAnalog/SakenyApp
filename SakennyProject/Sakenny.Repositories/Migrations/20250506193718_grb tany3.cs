using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class grbtany3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId1",
                table: "PostFavouriteLists");

            migrationBuilder.DropIndex(
                name: "IX_PostFavouriteLists_FavouriteListId1",
                table: "PostFavouriteLists");

            migrationBuilder.DropColumn(
                name: "FavouriteListId1",
                table: "PostFavouriteLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavouriteListId1",
                table: "PostFavouriteLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_FavouriteListId1",
                table: "PostFavouriteLists",
                column: "FavouriteListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId1",
                table: "PostFavouriteLists",
                column: "FavouriteListId1",
                principalTable: "FavouriteLists",
                principalColumn: "Id");
        }
    }
}
