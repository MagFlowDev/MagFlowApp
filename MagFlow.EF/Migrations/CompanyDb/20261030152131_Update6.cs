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
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Stocktakes",
                type: "nvarchar(max)",
                nullable: true);

            // Existing records are not Added entries, so SaveChangesAsync will not generate their codes.
            migrationBuilder.Sql(@"
                UPDATE [Stocktakes]
                SET [Code] = CONCAT(
                    'INV-', RIGHT(CONVERT(varchar(4), YEAR([CreatedAt])), 2), '-',
                    CASE WHEN [Id] < 10000
                        THEN RIGHT('0000' + CONVERT(varchar(10), [Id]), 4)
                        ELSE CONVERT(varchar(10), [Id])
                    END)
                WHERE [Code] IS NULL OR [Code] = ''; ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Stocktakes");
        }
    }
}
