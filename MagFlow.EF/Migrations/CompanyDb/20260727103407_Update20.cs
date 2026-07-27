using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Warehouses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "Warehouses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemovedById",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CreatedById",
                table: "Warehouses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_RemovedById",
                table: "Warehouses",
                column: "RemovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Users_CreatedById",
                table: "Warehouses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Users_RemovedById",
                table: "Warehouses",
                column: "RemovedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Users_CreatedById",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Users_RemovedById",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_CreatedById",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_RemovedById",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Warehouses");
        }
    }
}
