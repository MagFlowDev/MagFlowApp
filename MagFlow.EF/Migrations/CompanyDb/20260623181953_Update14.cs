using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductTypes_TypeId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_TypeId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "IsBasic",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ProductCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBasic",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_CategoryId",
                table: "ProductTypes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_ProductCategories_CategoryId",
                table: "ProductTypes",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_ProductCategories_CategoryId",
                table: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypes_CategoryId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "IsBasic",
                table: "ProductCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsBasic",
                table: "ProductTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ProductCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_TypeId",
                table: "ProductCategories",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductTypes_TypeId",
                table: "ProductCategories",
                column: "TypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id");
        }
    }
}
