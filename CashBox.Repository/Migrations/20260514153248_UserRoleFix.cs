using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER");

            migrationBuilder.DropColumn(
                name: "STATE_ID",
                table: "SYS_USER_ROLE");

            migrationBuilder.AlterColumn<int>(
                name: "ORGANIZATION_ID",
                table: "SYS_USER",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SYS_USER_ROLE",
                table: "SYS_USER_ROLE");

            migrationBuilder.AddColumn<int>(
                name: "STATE_ID",
                table: "SYS_USER_ROLE",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ORGANIZATION_ID",
                table: "SYS_USER",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SYS_USER_ROLE",
                table: "SYS_USER_ROLE",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ROLE_STATE_ID",
                table: "SYS_USER_ROLE",
                column: "STATE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USER_ROLE_USER_ID",
                table: "SYS_USER_ROLE",
                column: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_USER_INFO_ORGANIZATION_ORGANIZATION_ID",
                table: "SYS_USER",
                column: "ORGANIZATION_ID",
                principalTable: "INFO_ORGANIZATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_USER_ROLE_ENUM_STATE_STATE_ID",
                table: "SYS_USER_ROLE",
                column: "STATE_ID",
                principalTable: "ENUM_STATE",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
