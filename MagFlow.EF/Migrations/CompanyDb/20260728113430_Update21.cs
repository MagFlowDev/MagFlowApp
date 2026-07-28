using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentItems_WarehouseStorages_StorageId",
                table: "DocumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseStorages_StorageId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "WarehouseStorages");

            migrationBuilder.RenameColumn(
                name: "StorageId",
                table: "Items",
                newName: "WarehouseSectorRowSlotId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_StorageId",
                table: "Items",
                newName: "IX_Items_WarehouseSectorRowSlotId");

            migrationBuilder.AddColumn<int>(
                name: "RowId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
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

            migrationBuilder.CreateTable(
                name: "WarehouseSectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSectors_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSectorRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSectorRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSectorRows_WarehouseSectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "WarehouseSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSectorRowSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSectorRowSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSectorRowSlots_WarehouseSectorRows_RowId",
                        column: x => x.RowId,
                        principalTable: "WarehouseSectorRows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_RowId",
                table: "Items",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SectorId",
                table: "Items",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SlotId",
                table: "Items",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseSectorId",
                table: "Items",
                column: "WarehouseSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WarehouseSectorRowId",
                table: "Items",
                column: "WarehouseSectorRowId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSectorRows_SectorId",
                table: "WarehouseSectorRows",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSectorRowSlots_RowId",
                table: "WarehouseSectorRowSlots",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSectors_WarehouseId",
                table: "WarehouseSectors",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentItems_WarehouseSectors_StorageId",
                table: "DocumentItems",
                column: "StorageId",
                principalTable: "WarehouseSectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_SlotId",
                table: "Items",
                column: "SlotId",
                principalTable: "WarehouseSectorRowSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_WarehouseSectorRowSlotId",
                table: "Items",
                column: "WarehouseSectorRowSlotId",
                principalTable: "WarehouseSectorRowSlots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRows_RowId",
                table: "Items",
                column: "RowId",
                principalTable: "WarehouseSectorRows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectorRows_WarehouseSectorRowId",
                table: "Items",
                column: "WarehouseSectorRowId",
                principalTable: "WarehouseSectorRows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectors_SectorId",
                table: "Items",
                column: "SectorId",
                principalTable: "WarehouseSectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseSectors_WarehouseSectorId",
                table: "Items",
                column: "WarehouseSectorId",
                principalTable: "WarehouseSectors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentItems_WarehouseSectors_StorageId",
                table: "DocumentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_SlotId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRowSlots_WarehouseSectorRowSlotId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRows_RowId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectorRows_WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectors_SectorId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_WarehouseSectors_WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "WarehouseSectorRowSlots");

            migrationBuilder.DropTable(
                name: "WarehouseSectorRows");

            migrationBuilder.DropTable(
                name: "WarehouseSectors");

            migrationBuilder.DropIndex(
                name: "IX_Items_RowId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SectorId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SlotId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseSectorId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WarehouseSectorRowId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "WarehouseSectorRowSlotId",
                table: "Items",
                newName: "StorageId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_WarehouseSectorRowSlotId",
                table: "Items",
                newName: "IX_Items_StorageId");

            migrationBuilder.CreateTable(
                name: "WarehouseStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStorages_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStorages_WarehouseId",
                table: "WarehouseStorages",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentItems_WarehouseStorages_StorageId",
                table: "DocumentItems",
                column: "StorageId",
                principalTable: "WarehouseStorages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_WarehouseStorages_StorageId",
                table: "Items",
                column: "StorageId",
                principalTable: "WarehouseStorages",
                principalColumn: "Id");
        }
    }
}
