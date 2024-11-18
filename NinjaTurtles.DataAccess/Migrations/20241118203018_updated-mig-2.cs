using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedmig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerQrVerification_Customer_CustomerId",
                table: "CustomerQrVerification");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerQrVerification_QrCodeMain_QrCodeMainId1",
                table: "CustomerQrVerification");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLog_QrCodeMain_QrCodeMainId1",
                table: "QrLog");

            migrationBuilder.DropIndex(
                name: "IX_QrLog_QrCodeMainId1",
                table: "QrLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomerQrVerification_CustomerId",
                table: "CustomerQrVerification");

            migrationBuilder.DropIndex(
                name: "IX_CustomerQrVerification_QrCodeMainId1",
                table: "CustomerQrVerification");

            migrationBuilder.DropColumn(
                name: "QrCodeMainId1",
                table: "QrLog");

            migrationBuilder.DropColumn(
                name: "QrCodeMainId1",
                table: "CustomerQrVerification");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QrCodeMain",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QrCodeMain");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customer");

            migrationBuilder.AddColumn<Guid>(
                name: "QrCodeMainId1",
                table: "QrLog",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QrCodeMainId1",
                table: "CustomerQrVerification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QrLog_QrCodeMainId1",
                table: "QrLog",
                column: "QrCodeMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQrVerification_CustomerId",
                table: "CustomerQrVerification",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQrVerification_QrCodeMainId1",
                table: "CustomerQrVerification",
                column: "QrCodeMainId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerQrVerification_Customer_CustomerId",
                table: "CustomerQrVerification",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerQrVerification_QrCodeMain_QrCodeMainId1",
                table: "CustomerQrVerification",
                column: "QrCodeMainId1",
                principalTable: "QrCodeMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QrLog_QrCodeMain_QrCodeMainId1",
                table: "QrLog",
                column: "QrCodeMainId1",
                principalTable: "QrCodeMain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
