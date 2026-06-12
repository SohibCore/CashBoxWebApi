using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BuildNewReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INCOME_REPORT");

            migrationBuilder.DropTable(
                name: "OUTCOME_REPORT");

            migrationBuilder.DropTable(
                name: "DOCUMENT_REPORT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOCUMENT_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    STATUS_ID = table.Column<int>(type: "integer", nullable: false),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    CLOSING_CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CLOSING_DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    OPENING_CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    OPENING_DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_REPORT_ENUM_STATUS_STATUS_ID",
                        column: x => x.STATUS_ID,
                        principalTable: "ENUM_STATUS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_REPORT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INCOME_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DocumentReportId = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INCOME_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INCOME_REPORT_DOCUMENT_REPORT_DocumentReportId",
                        column: x => x.DocumentReportId,
                        principalTable: "DOCUMENT_REPORT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_INCOME_REPORT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUTCOME_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CREDIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DEBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    DocumentReportId = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTCOME_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OUTCOME_REPORT_DOCUMENT_REPORT_DocumentReportId",
                        column: x => x.DocumentReportId,
                        principalTable: "DOCUMENT_REPORT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OUTCOME_REPORT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_REPORT_STATUS_ID",
                table: "DOCUMENT_REPORT",
                column: "STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_REPORT_SUPPLIER_ID",
                table: "DOCUMENT_REPORT",
                column: "SUPPLIER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_REPORT_DocumentReportId",
                table: "INCOME_REPORT",
                column: "DocumentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_REPORT_SUPPLIER_ID",
                table: "INCOME_REPORT",
                column: "SUPPLIER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_REPORT_DocumentReportId",
                table: "OUTCOME_REPORT",
                column: "DocumentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_REPORT_SUPPLIER_ID",
                table: "OUTCOME_REPORT",
                column: "SUPPLIER_ID");
        }
    }
}
