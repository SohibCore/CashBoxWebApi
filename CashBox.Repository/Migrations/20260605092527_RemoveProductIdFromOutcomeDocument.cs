using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductIdFromOutcomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OUTCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropIndex(
                name: "IX_OUTCOME_DOCUMENT_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "PRICE",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "PRODUCT_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "QUANTITY",
                table: "OUTCOME_DOCUMENT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PRICE",
                table: "OUTCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PRODUCT_ID",
                table: "OUTCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QUANTITY",
                table: "OUTCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT",
                column: "PRODUCT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OUTCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT",
                column: "PRODUCT_ID",
                principalTable: "SYS_PRODUCT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
