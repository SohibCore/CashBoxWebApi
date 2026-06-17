using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACCOUNT",
                table: "SYS_SUPPLIER",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ADDRESS",
                table: "SYS_SUPPLIER",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DIRECTOR",
                table: "SYS_SUPPLIER",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MFO",
                table: "SYS_SUPPLIER",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NAME",
                table: "SYS_SUPPLIER",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACCOUNT",
                table: "SYS_SUPPLIER");

            migrationBuilder.DropColumn(
                name: "ADDRESS",
                table: "SYS_SUPPLIER");

            migrationBuilder.DropColumn(
                name: "DIRECTOR",
                table: "SYS_SUPPLIER");

            migrationBuilder.DropColumn(
                name: "MFO",
                table: "SYS_SUPPLIER");

            migrationBuilder.DropColumn(
                name: "NAME",
                table: "SYS_SUPPLIER");
        }
    }
}
