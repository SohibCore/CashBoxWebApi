using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrgIdFromIncomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropIndex(
                name: "IX_INCOME_DOCUMENT_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
