using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationIdIncomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "OUTCOME_REPORT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "INCOME_REPORT",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "OUTCOME_REPORT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "INCOME_REPORT");
        }
    }
}
