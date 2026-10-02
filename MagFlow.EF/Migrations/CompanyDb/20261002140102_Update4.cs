using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StocktakeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StocktakeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: true),
                    RowId = table.Column<int>(type: "int", nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    DefaultUnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakeItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StocktakeItems_Stocktakes_StocktakeId",
                        column: x => x.StocktakeId,
                        principalTable: "Stocktakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakeItems_Units_DefaultUnitId",
                        column: x => x.DefaultUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakeItems_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StocktakeItems_WarehouseSectorRowSlots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "WarehouseSectorRowSlots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StocktakeItems_WarehouseSectorRows_RowId",
                        column: x => x.RowId,
                        principalTable: "WarehouseSectorRows",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StocktakeItems_WarehouseSectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "WarehouseSectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StocktakeItems_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StocktakeItemParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ParameterId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakeItemParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakeItemParameters_CustomParameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "CustomParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakeItemParameters_StocktakeItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "StocktakeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItemParameters_ItemId",
                table: "StocktakeItemParameters",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItemParameters_ParameterId",
                table: "StocktakeItemParameters",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_CreatedById",
                table: "StocktakeItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_DefaultUnitId",
                table: "StocktakeItems",
                column: "DefaultUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_ProductId",
                table: "StocktakeItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_RowId",
                table: "StocktakeItems",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_SectorId",
                table: "StocktakeItems",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_SlotId",
                table: "StocktakeItems",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_StocktakeId",
                table: "StocktakeItems",
                column: "StocktakeId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeItems_WarehouseId",
                table: "StocktakeItems",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StocktakeItemParameters");

            migrationBuilder.DropTable(
                name: "StocktakeItems");
        }
    }
}
