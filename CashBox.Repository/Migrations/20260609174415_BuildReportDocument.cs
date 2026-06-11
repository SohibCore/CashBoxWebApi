using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BuildReportDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOCUMENT_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    OPENING_DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    OPENING_CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CLOSING_DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CLOSING_CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_REPORT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENT_TURNOVER_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false),
                    DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocumentReportId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_TURNOVER_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_TURNOVER_REPORT_DOCUMENT_REPORT_DocumentReportId",
                        column: x => x.DocumentReportId,
                        principalTable: "DOCUMENT_REPORT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DOCUMENT_TURNOVER_REPORT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_REPORT_SUPPLIER_ID",
                table: "DOCUMENT_REPORT",
                column: "SUPPLIER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TURNOVER_REPORT_DocumentReportId",
                table: "DOCUMENT_TURNOVER_REPORT",
                column: "DocumentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TURNOVER_REPORT_SUPPLIER_ID",
                table: "DOCUMENT_TURNOVER_REPORT",
                column: "SUPPLIER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCUMENT_TURNOVER_REPORT");

            migrationBuilder.DropTable(
                name: "DOCUMENT_REPORT");
        }
    }
}
