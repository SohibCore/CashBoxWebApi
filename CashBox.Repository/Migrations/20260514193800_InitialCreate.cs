using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ENUM_STATE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CODE = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENUM_STATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INFO_CURRENCY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_CURRENCY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INFO_DISTRICT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CODE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    REGION = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_DISTRICT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INFO_REGION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CODE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_REGION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ROLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INFO_CURRENCY_RATE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RATE = table.Column<decimal>(type: "numeric", nullable: false),
                    DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CURRENCY_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_CURRENCY_RATE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INFO_CURRENCY_RATE_INFO_CURRENCY_CURRENCY_ID",
                        column: x => x.CURRENCY_ID,
                        principalTable: "INFO_CURRENCY",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HL_CONTRACTOR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    INN = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    PINFL = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    REGION_ID = table.Column<int>(type: "integer", nullable: false),
                    DISTRICT_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL_CONTRACTOR", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HL_CONTRACTOR_INFO_DISTRICT_DISTRICT_ID",
                        column: x => x.DISTRICT_ID,
                        principalTable: "INFO_DISTRICT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HL_CONTRACTOR_INFO_REGION_REGION_ID",
                        column: x => x.REGION_ID,
                        principalTable: "INFO_REGION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INFO_ORGANIZATION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    INN = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    REGION_ID = table.Column<int>(type: "integer", nullable: false),
                    DISTRICT = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INFO_ORGANIZATION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INFO_ORGANIZATION_INFO_REGION_REGION_ID",
                        column: x => x.REGION_ID,
                        principalTable: "INFO_REGION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HL_CONTRACTOR_ACCOUNT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CODE = table.Column<string>(type: "character varying(27)", maxLength: 27, nullable: false),
                    CONTRACTOR_ID = table.Column<int>(type: "integer", nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL_CONTRACTOR_ACCOUNT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HL_CONTRACTOR_ACCOUNT_HL_CONTRACTOR_CONTRACTOR_ID",
                        column: x => x.CONTRACTOR_ID,
                        principalTable: "HL_CONTRACTOR",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PINFL = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ADDRESS = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ORGANIZATION_ID = table.Column<int>(type: "integer", nullable: true),
                    DATE_OF_BIRTH = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PASSPORT_SERIES = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    EMAIL = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                        column: x => x.ORGANIZATION_ID,
                        principalTable: "INFO_ORGANIZATION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_ROLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<int>(type: "integer", nullable: false),
                    ROLE_ID = table.Column<int>(type: "integer", nullable: false),
                    CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
                    MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_ROLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_USER_ROLE_SYS_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "SYS_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_USER_ROLE_SYS_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "SYS_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HL_CONTRACTOR_DISTRICT_ID",
                table: "HL_CONTRACTOR",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HL_CONTRACTOR_REGION_ID",
                table: "HL_CONTRACTOR",
                column: "REGION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HL_CONTRACTOR_ACCOUNT_CONTRACTOR_ID",
                table: "HL_CONTRACTOR_ACCOUNT",
                column: "CONTRACTOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_CURRENCY_RATE_CURRENCY_ID",
                table: "INFO_CURRENCY_RATE",
                column: "CURRENCY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INFO_ORGANIZATION_REGION_ID",
                table: "INFO_ORGANIZATION",
                column: "REGION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ORGANIZATION_ID",
                table: "SYS_USER",
                column: "ORGANIZATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ROLE_ROLE_ID",
                table: "SYS_USER_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ROLE_USER_ID",
                table: "SYS_USER_ROLE",
                column: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ENUM_STATE");

            migrationBuilder.DropTable(
                name: "HL_CONTRACTOR_ACCOUNT");

            migrationBuilder.DropTable(
                name: "INFO_CURRENCY_RATE");

            migrationBuilder.DropTable(
                name: "SYS_USER_ROLE");

            migrationBuilder.DropTable(
                name: "HL_CONTRACTOR");

            migrationBuilder.DropTable(
                name: "INFO_CURRENCY");

            migrationBuilder.DropTable(
                name: "SYS_ROLE");

            migrationBuilder.DropTable(
                name: "SYS_USER");

            migrationBuilder.DropTable(
                name: "INFO_DISTRICT");

            migrationBuilder.DropTable(
                name: "INFO_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "INFO_REGION");
        }
    }
}
