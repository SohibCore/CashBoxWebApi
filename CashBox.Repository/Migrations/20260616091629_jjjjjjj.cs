using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class jjjjjjj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT",
                table: "INFO_ORGANIZATION");

            migrationBuilder.RenameColumn(
                name: "DISTRICT",
                table: "INFO_ORGANIZATION",
                newName: "DISTRICT_ID");

            migrationBuilder.RenameIndex(
                name: "IX_INFO_ORGANIZATION_DISTRICT",
                table: "INFO_ORGANIZATION",
                newName: "IX_INFO_ORGANIZATION_DISTRICT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT_ID",
                principalTable: "INFO_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION");

            migrationBuilder.RenameColumn(
                name: "DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                newName: "DISTRICT");

            migrationBuilder.RenameIndex(
                name: "IX_INFO_ORGANIZATION_DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                newName: "IX_INFO_ORGANIZATION_DISTRICT");

            migrationBuilder.AddForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT",
                principalTable: "INFO_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
