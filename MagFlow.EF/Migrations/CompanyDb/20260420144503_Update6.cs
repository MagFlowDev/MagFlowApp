using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Claim_ClaimId",
                table: "RoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claim",
                table: "Claim");

            migrationBuilder.RenameTable(
                name: "Claim",
                newName: "Claims");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Policy",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claims",
                table: "Claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Claims_ClaimId",
                table: "RoleClaims",
                column: "ClaimId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Claims_ClaimId",
                table: "RoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claims",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Policy",
                table: "Claims");

            migrationBuilder.RenameTable(
                name: "Claims",
                newName: "Claim");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claim",
                table: "Claim",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Claim_ClaimId",
                table: "RoleClaims",
                column: "ClaimId",
                principalTable: "Claim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
