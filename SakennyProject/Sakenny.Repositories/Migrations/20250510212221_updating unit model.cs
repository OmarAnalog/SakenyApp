using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sakenny.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatingunitmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "RentPrice",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BedRoomCount",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenderType",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "BedRoomCount",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "GenderType",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Units",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Units",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "RentPrice",
                table: "Units",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
