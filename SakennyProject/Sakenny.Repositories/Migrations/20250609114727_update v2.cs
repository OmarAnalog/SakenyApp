using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceToken_AspNetUsers_UserId",
                table: "DeviceToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceToken",
                table: "DeviceToken");

            migrationBuilder.RenameTable(
                name: "DeviceToken",
                newName: "DeviceTokens");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceToken_UserId",
                table: "DeviceTokens",
                newName: "IX_DeviceTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceTokens",
                table: "DeviceTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTokens_AspNetUsers_UserId",
                table: "DeviceTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTokens_AspNetUsers_UserId",
                table: "DeviceTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceTokens",
                table: "DeviceTokens");

            migrationBuilder.RenameTable(
                name: "DeviceTokens",
                newName: "DeviceToken");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceTokens_UserId",
                table: "DeviceToken",
                newName: "IX_DeviceToken_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceToken",
                table: "DeviceToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceToken_AspNetUsers_UserId",
                table: "DeviceToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
