using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RebuildOutcomeDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.RenameColumn(
                name: "TOTAL_SUM",
                table: "OUTCOME_DOCUMENT",
                newName: "DOC_SUM");

            migrationBuilder.RenameColumn(
                name: "DATE",
                table: "OUTCOME_DOCUMENT",
                newName: "DOC_ON");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "OUTCOME_DOCUMENT",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATED_AT",
                table: "OUTCOME_DOCUMENT",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CREATE_USER_ID",
                table: "OUTCOME_DOCUMENT",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MODIFIED_AT",
                table: "OUTCOME_DOCUMENT",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MODIFIED_USER_ID",
                table: "OUTCOME_DOCUMENT",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "OUTCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "OUTCOME_DOCUMENT_TABLE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OWNER_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "integer", nullable: false),
                    PRICE = table.Column<decimal>(type: "numeric", nullable: false),
                    QUANTITY = table.Column<decimal>(type: "numeric", nullable: false),
                    TOTAL_SUM = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTCOME_DOCUMENT_TABLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OUTCOME_DOCUMENT_TABLE_OUTCOME_DOCUMENT_OWNER_ID",
                        column: x => x.OWNER_ID,
                        principalTable: "OUTCOME_DOCUMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUTCOME_DOCUMENT_TABLE_SYS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "SYS_PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_TABLE_OWNER_ID",
                table: "OUTCOME_DOCUMENT_TABLE",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_TABLE_PRODUCT_ID",
                table: "OUTCOME_DOCUMENT_TABLE",
                column: "PRODUCT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropTable(
                name: "OUTCOME_DOCUMENT_TABLE");

            migrationBuilder.DropColumn(
                name: "CREATED_AT",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USER_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "MODIFIED_AT",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "MODIFIED_USER_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.RenameColumn(
                name: "DOC_SUM",
                table: "OUTCOME_DOCUMENT",
                newName: "TOTAL_SUM");

            migrationBuilder.RenameColumn(
                name: "DOC_ON",
                table: "OUTCOME_DOCUMENT",
                newName: "DATE");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "OUTCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
