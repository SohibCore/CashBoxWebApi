using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SyncState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PASSWORD",
                table: "SYS_USER",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DATE_OF_BIRTH",
                table: "SYS_USER",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            //migrationBuilder.AddColumn<string>(
            //    name: "EMAIL",
            //    table: "SYS_USER",
            //    type: "character varying(150)",
            //    maxLength: 150,
            //    nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_REGION",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_REGION",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_CURRENCY_RATE",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_CURRENCY_RATE",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_CURRENCY",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_CURRENCY",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "HL_CONTRACTOR_ACCOUNT",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "HL_CONTRACTOR_ACCOUNT",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            //migrationBuilder.CreateTable(
            //    name: "ENUM_STATE",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        FULL_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
            //        SHORT_NAME = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
            //        CODE = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
            //        CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
            //        CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //        MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
            //        MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ENUM_STATE", x => x.ID);
            //    });

        //    migrationBuilder.CreateTable(
        //        name: "SYS_ROLE",
        //        columns: table => new
        //        {
        //            ID = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            NAME = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
        //            DESCRIPTION = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
        //            CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
        //            CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        //            MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
        //            MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SYS_ROLE", x => x.ID);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "SYS_USER_ROLE",
        //        columns: table => new
        //        {
        //            ID = table.Column<int>(type: "integer", nullable: false)
        //                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
        //            USER_ID = table.Column<int>(type: "integer", nullable: false),
        //            ROLE_ID = table.Column<int>(type: "integer", nullable: false),
        //            STATE_ID = table.Column<int>(type: "integer", nullable: false),
        //            CREATE_USER_ID = table.Column<int>(type: "integer", nullable: true),
        //            CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        //            MODIFIED_USER_ID = table.Column<int>(type: "integer", nullable: true),
        //            MODIFIED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_SYS_USER_ROLE", x => x.ID);
        //            table.ForeignKey(
        //                name: "FK_SYS_USER_ROLE_ENUM_STATE_STATE_ID",
        //                column: x => x.STATE_ID,
        //                principalTable: "ENUM_STATE",
        //                principalColumn: "ID",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_SYS_USER_ROLE_SYS_ROLE_ROLE_ID",
        //                column: x => x.ROLE_ID,
        //                principalTable: "SYS_ROLE",
        //                principalColumn: "ID",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_SYS_USER_ROLE_SYS_USER_USER_ID",
        //                column: x => x.USER_ID,
        //                principalTable: "SYS_USER",
        //                principalColumn: "ID",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SYS_USER_ROLE_ROLE_ID",
        //        table: "SYS_USER_ROLE",
        //        column: "ROLE_ID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SYS_USER_ROLE_STATE_ID",
        //        table: "SYS_USER_ROLE",
        //        column: "STATE_ID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_SYS_USER_ROLE_USER_ID",
        //        table: "SYS_USER_ROLE",
        //        column: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_USER_ROLE");

            migrationBuilder.DropTable(
                name: "ENUM_STATE");

            migrationBuilder.DropTable(
                name: "SYS_ROLE");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "SYS_USER");

            migrationBuilder.AlterColumn<string>(
                name: "PASSWORD",
                table: "SYS_USER",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATE_OF_BIRTH",
                table: "SYS_USER",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_REGION",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_REGION",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_CURRENCY_RATE",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_CURRENCY_RATE",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "INFO_CURRENCY",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "INFO_CURRENCY",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CREATE_USER_ID",
                table: "HL_CONTRACTOR_ACCOUNT",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATED_AT",
                table: "HL_CONTRACTOR_ACCOUNT",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
