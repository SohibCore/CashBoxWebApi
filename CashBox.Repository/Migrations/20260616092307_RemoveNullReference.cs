using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNullReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION");

            migrationBuilder.AlterColumn<int>(
                name: "DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT_ID",
                principalTable: "INFO_DISTRICT",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION");

            migrationBuilder.AlterColumn<int>(
                name: "DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_INFO_ORGANIZATION_INFO_DISTRICT_DISTRICT_ID",
                table: "INFO_ORGANIZATION",
                column: "DISTRICT_ID",
                principalTable: "INFO_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
