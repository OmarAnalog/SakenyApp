using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutherId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AutherId",
                table: "Notifications",
                column: "AutherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_AutherId",
                table: "Notifications",
                column: "AutherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_AutherId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AutherId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AutherId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");
        }
    }
}
