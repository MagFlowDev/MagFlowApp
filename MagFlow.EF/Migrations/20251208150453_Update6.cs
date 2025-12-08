using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModulePackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    TotalPricePerMonth = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    TotalPricePerYear = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModulePackageModulePricing",
                columns: table => new
                {
                    ModulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackagesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePackageModulePricing", x => new { x.ModulesId, x.PackagesId });
                    table.ForeignKey(
                        name: "FK_ModulePackageModulePricing_ModulePackages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "ModulePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModulePackageModulePricing_ModulePricing_ModulesId",
                        column: x => x.ModulesId,
                        principalTable: "ModulePricing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulePackageModulePricing_PackagesId",
                table: "ModulePackageModulePricing",
                column: "PackagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModulePackageModulePricing");

            migrationBuilder.DropTable(
                name: "ModulePackages");
        }
    }
}
