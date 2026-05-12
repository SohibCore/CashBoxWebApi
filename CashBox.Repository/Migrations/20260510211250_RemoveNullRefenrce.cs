using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashBox.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNullRefenrce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ADRESS",
                table: "SYS_USER",
                newName: "ADDRESS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ADDRESS",
                table: "SYS_USER",
                newName: "ADRESS");
        }
    }
}
