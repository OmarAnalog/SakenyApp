using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addingindexoncolumnunitratingidrating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UnitId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UnitId_RatingUserId",
                table: "Ratings",
                columns: new[] { "UnitId", "RatingUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UnitId_RatingUserId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UnitId",
                table: "Ratings",
                column: "UnitId");
        }
    }
}
