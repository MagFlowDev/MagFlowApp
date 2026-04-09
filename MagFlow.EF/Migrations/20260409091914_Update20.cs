using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "UserSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_CompanyId",
                table: "UserSessions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Companies_CompanyId",
                table: "UserSessions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Companies_CompanyId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_CompanyId",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserSessions");
        }
    }
}
