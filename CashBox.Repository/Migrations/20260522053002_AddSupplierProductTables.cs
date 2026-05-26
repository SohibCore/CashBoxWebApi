using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierProductTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CREATE_USER_ID",
                table: "SYS_USER",
                newName: "CREATED_USER_ID");

            migrationBuilder.CreateTable(
                name: "SYS_PRODUCT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    CODE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: false),
                    DELIVERED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PRODUCT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_PRODUCT_INFO_ORGANIZATION_ORGANIZATION_ID",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "INFO_ORGANIZATION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SUPPLIER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    INN = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CODE = table.Column<string>(type: "text", nullable: false),
                    CREATED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SUPPLIER", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_PRODUCT_ORGANIZATION_ID",
                table: "SYS_PRODUCT",
                column: "ORGANIZATION_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_PRODUCT");

            migrationBuilder.DropTable(
                name: "SYS_SUPPLIER");

            migrationBuilder.RenameColumn(
                name: "CREATED_USER_ID",
                table: "SYS_USER",
                newName: "CREATE_USER_ID");
        }
    }
}
