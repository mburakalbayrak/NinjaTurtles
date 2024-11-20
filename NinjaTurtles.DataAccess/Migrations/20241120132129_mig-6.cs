using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "CompanyOrderDetail",
                newName: "LicenceUnitPrice");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "QrCodeDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CompanyOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "QrCodeDetail");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CompanyOrderDetail");

            migrationBuilder.RenameColumn(
                name: "LicenceUnitPrice",
                table: "CompanyOrderDetail",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<string>(
                name: "QuantityPerUnit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
