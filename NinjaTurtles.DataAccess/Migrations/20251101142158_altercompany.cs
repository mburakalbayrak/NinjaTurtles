using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class altercompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyOrderDetail",
                table: "QrCodeMain",
                newName: "CompanyOrderDetailId");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Company");

            migrationBuilder.RenameColumn(
                name: "CompanyOrderDetailId",
                table: "QrCodeMain",
                newName: "CompanyOrderDetail");
        }
    }
}
