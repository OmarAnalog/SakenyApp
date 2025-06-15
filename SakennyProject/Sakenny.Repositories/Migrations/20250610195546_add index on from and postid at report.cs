using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addindexonfromandpostidatreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_FromId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_FromId_ContentId",
                table: "Reports",
                columns: new[] { "FromId", "ContentId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_FromId_ContentId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_FromId",
                table: "Reports",
                column: "FromId");
        }
    }
}
