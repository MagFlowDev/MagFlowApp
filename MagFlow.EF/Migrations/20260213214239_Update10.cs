using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagFlow.EF.Migrations
{
    /// <inheritdoc />
    public partial class Update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionModule_Modules_ModuleId",
                table: "SessionModule");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionModule_UserSessions_SessionId",
                table: "SessionModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionModule",
                table: "SessionModule");

            migrationBuilder.RenameTable(
                name: "SessionModule",
                newName: "SessionModules");

            migrationBuilder.RenameIndex(
                name: "IX_SessionModule_ModuleId",
                table: "SessionModules",
                newName: "IX_SessionModules_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionModules",
                table: "SessionModules",
                columns: new[] { "SessionId", "ModuleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessionModules_Modules_ModuleId",
                table: "SessionModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionModules_UserSessions_SessionId",
                table: "SessionModules",
                column: "SessionId",
                principalTable: "UserSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionModules_Modules_ModuleId",
                table: "SessionModules");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionModules_UserSessions_SessionId",
                table: "SessionModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionModules",
                table: "SessionModules");

            migrationBuilder.RenameTable(
                name: "SessionModules",
                newName: "SessionModule");

            migrationBuilder.RenameIndex(
                name: "IX_SessionModules_ModuleId",
                table: "SessionModule",
                newName: "IX_SessionModule_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionModule",
                table: "SessionModule",
                columns: new[] { "SessionId", "ModuleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SessionModule_Modules_ModuleId",
                table: "SessionModule",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionModule_UserSessions_SessionId",
                table: "SessionModule",
                column: "SessionId",
                principalTable: "UserSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
