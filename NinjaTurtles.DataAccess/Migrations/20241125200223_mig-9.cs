using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QrCodeDetail",
                table: "QrCodeDetail");

            migrationBuilder.RenameTable(
                name: "QrCodeDetail",
                newName: "QrCodeHumanDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QrCodeHumanDetail",
                table: "QrCodeHumanDetail",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QrCodeHumanDetail",
                table: "QrCodeHumanDetail");

            migrationBuilder.RenameTable(
                name: "QrCodeHumanDetail",
                newName: "QrCodeDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QrCodeDetail",
                table: "QrCodeDetail",
                column: "Id");
        }
    }
}
