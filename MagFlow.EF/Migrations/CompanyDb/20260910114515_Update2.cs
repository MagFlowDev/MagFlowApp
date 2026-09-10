using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    SourceWarehouseId = table.Column<int>(type: "int", nullable: true),
                    SourceSectorId = table.Column<int>(type: "int", nullable: true),
                    SourceRowId = table.Column<int>(type: "int", nullable: true),
                    SourceSlotId = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouseId = table.Column<int>(type: "int", nullable: true),
                    TargetSectorId = table.Column<int>(type: "int", nullable: true),
                    TargetRowId = table.Column<int>(type: "int", nullable: true),
                    TargetSlotId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    WarehouseSectorId = table.Column<int>(type: "int", nullable: true),
                    WarehouseSectorRowId = table.Column<int>(type: "int", nullable: true),
                    WarehouseSectorRowSlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovements_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRowSlots_SourceSlotId",
                        column: x => x.SourceSlotId,
                        principalTable: "WarehouseSectorRowSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRowSlots_TargetSlotId",
                        column: x => x.TargetSlotId,
                        principalTable: "WarehouseSectorRowSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRowSlots_WarehouseSectorRowSlotId",
                        column: x => x.WarehouseSectorRowSlotId,
                        principalTable: "WarehouseSectorRowSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRows_SourceRowId",
                        column: x => x.SourceRowId,
                        principalTable: "WarehouseSectorRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRows_TargetRowId",
                        column: x => x.TargetRowId,
                        principalTable: "WarehouseSectorRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectorRows_WarehouseSectorRowId",
                        column: x => x.WarehouseSectorRowId,
                        principalTable: "WarehouseSectorRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectors_SourceSectorId",
                        column: x => x.SourceSectorId,
                        principalTable: "WarehouseSectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectors_TargetSectorId",
                        column: x => x.TargetSectorId,
                        principalTable: "WarehouseSectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_WarehouseSectors_WarehouseSectorId",
                        column: x => x.WarehouseSectorId,
                        principalTable: "WarehouseSectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_Warehouses_SourceWarehouseId",
                        column: x => x.SourceWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_Warehouses_TargetWarehouseId",
                        column: x => x.TargetWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovements_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatedById",
                table: "StockMovements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_DocumentId",
                table: "StockMovements",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ItemId",
                table: "StockMovements",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SourceRowId",
                table: "StockMovements",
                column: "SourceRowId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SourceSectorId",
                table: "StockMovements",
                column: "SourceSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SourceSlotId",
                table: "StockMovements",
                column: "SourceSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_SourceWarehouseId",
                table: "StockMovements",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TargetRowId",
                table: "StockMovements",
                column: "TargetRowId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TargetSectorId",
                table: "StockMovements",
                column: "TargetSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TargetSlotId",
                table: "StockMovements",
                column: "TargetSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TargetWarehouseId",
                table: "StockMovements",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseId",
                table: "StockMovements",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseSectorId",
                table: "StockMovements",
                column: "WarehouseSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseSectorRowId",
                table: "StockMovements",
                column: "WarehouseSectorRowId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseSectorRowSlotId",
                table: "StockMovements",
                column: "WarehouseSectorRowSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockMovements");
        }
    }
}
