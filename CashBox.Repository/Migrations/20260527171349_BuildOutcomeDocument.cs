using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BuildOutcomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ENUM_STATE");

            migrationBuilder.AddColumn<int>(
                name: "STATUS",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OUTCOME_DOCUMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SUPPLIER_ID = table.Column<int>(type: "integer", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    TOTAL_SUM = table.Column<decimal>(type: "numeric", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTCOME_DOCUMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OUTCOME_DOCUMENT_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUTCOME_DOCUMENT_SYS_SUPPLIER_SUPPLIER_ID",
                        column: x => x.SUPPLIER_ID,
                        principalTable: "SYS_SUPPLIER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_SUPPLIER_ID",
                table: "OUTCOME_DOCUMENT",
                column: "SUPPLIER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "INCOME_DOCUMENT");

            migrationBuilder.CreateTable(
                name: "ENUM_STATE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENUM_STATE", x => x.ID);
                });
        }
    }
}
