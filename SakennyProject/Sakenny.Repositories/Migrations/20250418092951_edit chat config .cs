using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class editchatconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_AspNetUsers_UserId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "SecondUserId",
                table: "Chats",
                newName: "SUserId");

            migrationBuilder.RenameColumn(
                name: "FirstUserId",
                table: "Chats",
                newName: "FUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_FirstUserId_SecondUserId",
                table: "Chats",
                newName: "IX_Chats_FUserId_SUserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Connections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SUserId",
                table: "Chats",
                column: "SUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_FUserId",
                table: "Chats",
                column: "FUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_SUserId",
                table: "Chats",
                column: "SUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_AspNetUsers_UserId",
                table: "Connections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_FUserId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_SUserId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_AspNetUsers_UserId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Chats_SUserId",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "SUserId",
                table: "Chats",
                newName: "SecondUserId");

            migrationBuilder.RenameColumn(
                name: "FUserId",
                table: "Chats",
                newName: "FirstUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_FUserId_SUserId",
                table: "Chats",
                newName: "IX_Chats_FirstUserId_SecondUserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Connections",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_AspNetUsers_UserId",
                table: "Connections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
