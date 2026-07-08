using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductComponents_Products_ProductId1",
                table: "ProductComponents");

            migrationBuilder.DropIndex(
                name: "IX_ProductComponents_ProductId1",
                table: "ProductComponents");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductComponents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "ProductComponents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponents_ProductId1",
                table: "ProductComponents",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComponents_Products_ProductId1",
                table: "ProductComponents",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
