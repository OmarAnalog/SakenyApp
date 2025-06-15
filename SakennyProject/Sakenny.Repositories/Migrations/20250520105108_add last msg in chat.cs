using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addlastmsginchat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastMsg",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMsg",
                table: "Chats");
        }
    }
}
