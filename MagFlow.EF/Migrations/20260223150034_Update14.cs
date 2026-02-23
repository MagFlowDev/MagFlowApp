using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line1",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Line2",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyLogo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLogo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyLogo_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanySettings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLogo_CompanyId",
                table: "CompanyLogo",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyLogo");

            migrationBuilder.DropTable(
                name: "CompanySettings");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address_Line1",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address_Line2",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Companies");
        }
    }
}
