using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RebuildIncomeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Column nomlarini o'zgartirish
            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "OUTCOME_DOCUMENT",
                newName: "STATUS_ID");

            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "INCOME_DOCUMENT",
                newName: "STATUS_ID");

            migrationBuilder.RenameColumn(
                name: "DATE",
                table: "INCOME_DOCUMENT",
                newName: "DOC_ON");

            // 2. INCOME_DOCUMENT_TABLE columnlarini o'zgartirish
            migrationBuilder.AlterColumn<long>(
                name: "OWNER_ID",
                table: "INCOME_DOCUMENT_TABLE",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "INCOME_DOCUMENT_TABLE",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "TOTAL_SUM",
                table: "INCOME_DOCUMENT_TABLE",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            // 3. INCOME_DOCUMENT columnlarini o'zgartirish
            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "INCOME_DOCUMENT",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CREATED_AT",
                table: "INCOME_DOCUMENT",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CREATE_USER_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DOC_SUM",
                table: "INCOME_DOCUMENT",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "MODIFIED_AT",
                table: "INCOME_DOCUMENT",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MODIFIED_USER_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // 4. ENUM_STATUS table yaratish
            migrationBuilder.CreateTable(
                name: "ENUM_STATUS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENUM_STATUS", x => x.ID);
                });

            // 5. FIX-1: ENUM_STATUS ga default qiymatlar qo'shish
            // FK qo'shilishidan OLDIN bo'lishi shart!
            migrationBuilder.Sql(@"
    INSERT INTO ""ENUM_STATUS"" (""CODE"", ""SHORT_NAME"", ""FULL_NAME"") 
    VALUES 
    ('CREATED',     'Yaratilgan',      'Yaratilgan holat'),
    ('ACCEPT',      'Qabul qilingan',  'Qabul qilingan holat'),
    ('NOT_ACCEPT',  'Rad etilgan',     'Rad etilgan holat'),
    ('MODIFIED',    'O''zgartirilgan', 'O''zgartirilgan holat'),
    ('DELETE',      'O''chirilgan',    'O''chirilgan holat');
");

            // 6. FIX-2: STATUS_ID = 0 bo'lgan qatorlarni 1 (ACTIVE) ga o'zgartirish
            migrationBuilder.Sql(@"
                UPDATE ""INCOME_DOCUMENT"" 
                SET ""STATUS_ID"" = 1 
                WHERE ""STATUS_ID"" = 0;
            ");

            migrationBuilder.Sql(@"
                UPDATE ""OUTCOME_DOCUMENT"" 
                SET ""STATUS_ID"" = 1 
                WHERE ""STATUS_ID"" = 0;
            ");

            // 7. FIX-3: ORGANIZATION_ID = 0 bo'lgan qatorlarni to'g'rilash
            // INFO_ORGANIZATION dan birinchi mavjud ID ni oladi
            migrationBuilder.Sql(@"
                UPDATE ""INCOME_DOCUMENT"" 
                SET ""ORGANIZATION_ID"" = (
                    SELECT ""ID"" FROM ""INFO_ORGANIZATION"" ORDER BY ""ID"" LIMIT 1
                )
                WHERE ""ORGANIZATION_ID"" = 0;
            ");

            // 8. Index lar yaratish
            migrationBuilder.CreateIndex(
                name: "IX_OUTCOME_DOCUMENT_STATUS_ID",
                table: "OUTCOME_DOCUMENT",
                column: "STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INCOME_DOCUMENT_STATUS_ID",
                table: "INCOME_DOCUMENT",
                column: "STATUS_ID");

            // 9. Foreign key lar — eng oxirida, chunki data tayyor bo'lishi kerak
            migrationBuilder.AddForeignKey(
                name: "FK_INCOME_DOCUMENT_ENUM_STATUS_STATUS_ID",
                table: "INCOME_DOCUMENT",
                column: "STATUS_ID",
                principalTable: "ENUM_STATUS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OUTCOME_DOCUMENT_ENUM_STATUS_STATUS_ID",
                table: "OUTCOME_DOCUMENT",
                column: "STATUS_ID",
                principalTable: "ENUM_STATUS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_ENUM_STATUS_STATUS_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_INCOME_DOCUMENT_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropForeignKey(
                name: "FK_OUTCOME_DOCUMENT_ENUM_STATUS_STATUS_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropTable(
                name: "ENUM_STATUS");

            migrationBuilder.DropIndex(
                name: "IX_OUTCOME_DOCUMENT_STATUS_ID",
                table: "OUTCOME_DOCUMENT");

            migrationBuilder.DropIndex(
                name: "IX_INCOME_DOCUMENT_ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropIndex(
                name: "IX_INCOME_DOCUMENT_STATUS_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "TOTAL_SUM",
                table: "INCOME_DOCUMENT_TABLE");

            migrationBuilder.DropColumn(
                name: "CREATED_AT",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "CREATE_USER_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "DOC_SUM",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "MODIFIED_AT",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "MODIFIED_USER_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "INCOME_DOCUMENT");

            migrationBuilder.RenameColumn(
                name: "STATUS_ID",
                table: "OUTCOME_DOCUMENT",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "STATUS_ID",
                table: "INCOME_DOCUMENT",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "DOC_ON",
                table: "INCOME_DOCUMENT",
                newName: "DATE");

            migrationBuilder.AlterColumn<int>(
                name: "OWNER_ID",
                table: "INCOME_DOCUMENT_TABLE",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "INCOME_DOCUMENT_TABLE",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "INCOME_DOCUMENT",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}