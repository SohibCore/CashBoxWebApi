using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER");

            migrationBuilder.DropIndex(
                name: "IX_SYS_USER_ORGANIZATION_ID",
                table: "SYS_USER");

            migrationBuilder.DropColumn(
                name: "ORGANIZATION_ID",
                table: "SYS_USER");

            migrationBuilder.AlterColumn<string>(
                name: "SHORT_NAME",
                table: "SYS_USER",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "PINFL",
                table: "SYS_USER",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "PHONE_NUMBER",
                table: "SYS_USER",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "PASSPORT_SERIES",
                table: "SYS_USER",
                type: "character varying(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "FULL_NAME",
                table: "SYS_USER",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "DATE_OF_BIRTH",
                table: "SYS_USER",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ADRESS",
                table: "SYS_USER",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SHORT_NAME",
                table: "SYS_USER",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PINFL",
                table: "SYS_USER",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PHONE_NUMBER",
                table: "SYS_USER",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PASSPORT_SERIES",
                table: "SYS_USER",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FULL_NAME",
                table: "SYS_USER",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DATE_OF_BIRTH",
                table: "SYS_USER",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ADRESS",
                table: "SYS_USER",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ORGANIZATION_ID",
                table: "SYS_USER",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ORGANIZATION_ID",
                table: "SYS_USER",
                column: "ORGANIZATION_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
