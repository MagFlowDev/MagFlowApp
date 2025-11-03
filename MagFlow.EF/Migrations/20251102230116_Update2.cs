using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Companies_DefaultCompanyId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultCompanyId",
                table: "ApplicationUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Companies_DefaultCompanyId",
                table: "ApplicationUsers",
                column: "DefaultCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Companies_DefaultCompanyId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultCompanyId",
                table: "ApplicationUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Companies_DefaultCompanyId",
                table: "ApplicationUsers",
                column: "DefaultCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
