using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateIncomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INCOME_DOCUMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    TOTAL_SUM = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INCOME_DOCUMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "INFO_ORGANIZATION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_INCOME_DOCUMENT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_PRODUCT_ID",
                table: "INCOME_DOCUMENT",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_SUPPLIER_ID",
                table: "INCOME_DOCUMENT",
                column: "SUPPLIER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INCOME_DOCUMENT");
        }
    }
}
