using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateFormat",
                table: "ApplicationUserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecimalSeparator",
                table: "ApplicationUserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeFormat",
                table: "ApplicationUserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeZone",
                table: "ApplicationUserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFormat",
                table: "ApplicationUserSettings");

            migrationBuilder.DropColumn(
                name: "DecimalSeparator",
                table: "ApplicationUserSettings");

            migrationBuilder.DropColumn(
                name: "TimeFormat",
                table: "ApplicationUserSettings");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "ApplicationUserSettings");
        }
    }
}
