using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterQrLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
        name: "QrCodeMainId",
        table: "QrLog");

            migrationBuilder.AddColumn<Guid>(
                name: "QrCodeMainId",
                table: "QrLog",
                type: "uniqueidentifier",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QrCodeMainId",
                table: "QrLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
