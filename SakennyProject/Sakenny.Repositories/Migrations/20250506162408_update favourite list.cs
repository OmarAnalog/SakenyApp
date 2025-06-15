using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatefavouritelist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitFavouriteLists");

            migrationBuilder.CreateTable(
                name: "PostFavouriteLists",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FavouriteListId = table.Column<int>(type: "int", nullable: false),
                    PostId1 = table.Column<int>(type: "int", nullable: false),
                    FavouriteListId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFavouriteLists", x => new { x.FavouriteListId, x.PostId });
                    table.ForeignKey(
                        name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId",
                        column: x => x.FavouriteListId,
                        principalTable: "FavouriteLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostFavouriteLists_FavouriteLists_FavouriteListId1",
                        column: x => x.FavouriteListId1,
                        principalTable: "FavouriteLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostFavouriteLists_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostFavouriteLists_Posts_PostId1",
                        column: x => x.PostId1,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_FavouriteListId1",
                table: "PostFavouriteLists",
                column: "FavouriteListId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_PostId",
                table: "PostFavouriteLists",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavouriteLists_PostId1",
                table: "PostFavouriteLists",
                column: "PostId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostFavouriteLists");

            migrationBuilder.CreateTable(
                name: "UnitFavouriteLists",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    FavouriteListId = table.Column<int>(type: "int", nullable: false),
                    UnitId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitFavouriteLists", x => new { x.UnitId, x.FavouriteListId });
                    table.ForeignKey(
                        name: "FK_UnitFavouriteLists_FavouriteLists_FavouriteListId",
                        column: x => x.FavouriteListId,
                        principalTable: "FavouriteLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitFavouriteLists_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitFavouriteLists_Units_UnitId1",
                        column: x => x.UnitId1,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitFavouriteLists_FavouriteListId",
                table: "UnitFavouriteLists",
                column: "FavouriteListId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFavouriteLists_UnitId1",
                table: "UnitFavouriteLists",
                column: "UnitId1");
        }
    }
}
