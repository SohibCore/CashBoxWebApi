using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixedDistrictIdOrganizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DISTRICT",
                table: "INFO_ORGANIZATION",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.CreateIndex(
                name: "IX_INFO_ORGANIZATION_DISTRICT",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT");

            migrationBuilder.AddForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT",
                principalTable: "INFO_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT",
                table: "INFO_ORGANIZATION");

            migrationBuilder.DropIndex(
                name: "IX_INFO_ORGANIZATION_DISTRICT",
                table: "INFO_ORGANIZATION");

            migrationBuilder.AlterColumn<string>(
                name: "DISTRICT",
                table: "INFO_ORGANIZATION",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
