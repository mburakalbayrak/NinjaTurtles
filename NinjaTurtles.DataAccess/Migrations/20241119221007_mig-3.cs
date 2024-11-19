using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrId",
                table: "QrCodeMain",
                newName: "CompanyOrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "QrCodeMain",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QrCodeDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "QrCodeDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "QrCodeDetail");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "QrCodeDetail");

            migrationBuilder.RenameColumn(
                name: "CompanyOrderDetail",
                table: "QrCodeMain",
                newName: "QrId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "QrCodeMain",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
