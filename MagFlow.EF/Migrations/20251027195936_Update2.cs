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
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "UserSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "UserSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "UserSessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "UserSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "UserNotifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "UserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "UserNotifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "UserNotifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NIP",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "AuditLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OccuredAt",
                table: "AuditLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RelatedEntityId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApplicationRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogChange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditLogId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogChange_AuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccuredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventLogs_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: true),
                    RelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnabledFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnabledTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyModules_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModulePricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    PricePerYear = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePricing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulePricing_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyModulePricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    PricePerYear = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyModulePricing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyModulePricing_CompanyModules_CompanyModuleId",
                        column: x => x.CompanyModuleId,
                        principalTable: "CompanyModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogChange_AuditLogId",
                table: "AuditLogChange",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModulePricing_CompanyModuleId",
                table: "CompanyModulePricing",
                column: "CompanyModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_CompanyId",
                table: "CompanyModules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_ModuleId",
                table: "CompanyModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_UserId",
                table: "EventLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePricing_ModuleId",
                table: "ModulePricing",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Notification_NotificationId",
                table: "UserNotifications",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Notification_NotificationId",
                table: "UserNotifications");

            migrationBuilder.DropTable(
                name: "AuditLogChange");

            migrationBuilder.DropTable(
                name: "CompanyModulePricing");

            migrationBuilder.DropTable(
                name: "EventLogs");

            migrationBuilder.DropTable(
                name: "ModulePricing");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "CompanyModules");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "NIP",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "OccuredAt",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RelatedEntityId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApplicationRoles");
        }
    }
}
