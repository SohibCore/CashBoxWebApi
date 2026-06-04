using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BuildTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropIndex(
                name: "IX_INCOME_DOCUMENT_PRODUCT_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "PRICE",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "PRODUCT_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "QUANTITY",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "TOTAL_SUM",
                table: "INCOME_DOCUMENT");

            migrationBuilder.CreateTable(
                name: "INCOME_DOCUMENT_TABLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OWNER_ID = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INCOME_DOCUMENT_TABLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INCOME_DOCUMENT_TABLE_INCOME_DOCUMENT_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "INCOME_DOCUMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INCOME_DOCUMENT_TABLE_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_TABLE_OWNER_ID",
                table: "INCOME_DOCUMENT_TABLE",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_TABLE_PRODUCT_ID",
                table: "INCOME_DOCUMENT_TABLE",
                column: "PRODUCT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INCOME_DOCUMENT_TABLE");

            migrationBuilder.AddColumn<decimal>(
                name: "PRICE",
                table: "INCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PRODUCT_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QUANTITY",
                table: "INCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TOTAL_SUM",
                table: "INCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_PRODUCT_ID",
                table: "INCOME_DOCUMENT",
                column: "PRODUCT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_INCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                table: "INCOME_DOCUMENT",
                column: "PRODUCT_ID",
                principalTable: "SYS_PRODUCT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
