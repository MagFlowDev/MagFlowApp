using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contractors");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Contractors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contractors");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contractors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
