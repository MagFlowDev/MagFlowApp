using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VatRate",
                table: "Items",
                newName: "TaxRate");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DefaultUnitId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_DefaultUnitId",
                table: "Items",
                column: "DefaultUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Units_DefaultUnitId",
                table: "Items",
                column: "DefaultUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Units_DefaultUnitId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_DefaultUnitId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DefaultUnitId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "Items",
                newName: "VatRate");

            migrationBuilder.AlterColumn<int>(
                name: "WarehouseId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
