using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_WarehouseSectorRowSlotId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRows_WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectors_WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Warehouses_WarehouseId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseSectorRowSlotId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseSectorRowSlotId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseSectorId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseSectorRowId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseSectorRowSlotId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseId1",
                table: "Items",
                column: "WarehouseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseSectorId",
                table: "Items",
                column: "WarehouseSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseSectorRowId",
                table: "Items",
                column: "WarehouseSectorRowId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseSectorRowSlotId",
                table: "Items",
                column: "WarehouseSectorRowSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_WarehouseSectorRowSlotId",
                table: "Items",
                column: "WarehouseSectorRowSlotId",
                principalTable: "WarehouseSectorRowSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRows_WarehouseSectorRowId",
                table: "Items",
                column: "WarehouseSectorRowId",
                principalTable: "WarehouseSectorRows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectors_WarehouseSectorId",
                table: "Items",
                column: "WarehouseSectorId",
                principalTable: "WarehouseSectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Warehouses_WarehouseId1",
                table: "Items",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
