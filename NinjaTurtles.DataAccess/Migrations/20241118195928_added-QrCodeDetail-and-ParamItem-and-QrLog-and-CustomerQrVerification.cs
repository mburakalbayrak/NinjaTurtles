using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedQrCodeDetailandParamItemandQrLogandCustomerQrVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerQrVerification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    QrCodeMainId = table.Column<int>(type: "int", nullable: false),
                    VerificationTypeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifyDate = table.Column<bool>(type: "bit", nullable: false),
                    QrCodeMainId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerQrVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerQrVerification_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerQrVerification_QrCodeMain_QrCodeMainId1",
                        column: x => x.QrCodeMainId1,
                        principalTable: "QrCodeMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParamItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParamId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrCodeDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatusId = table.Column<int>(type: "int", nullable: true),
                    EducationStatusId = table.Column<int>(type: "int", nullable: true),
                    CityOfResidenceId = table.Column<int>(type: "int", nullable: true),
                    BloodTypeId = table.Column<int>(type: "int", nullable: true),
                    ProfessionId = table.Column<int>(type: "int", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegularMedications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryRelationId = table.Column<int>(type: "int", nullable: true),
                    SecondaryRelationId = table.Column<int>(type: "int", nullable: true),
                    PrimaryContactFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryContactFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodeDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogTypeId = table.Column<int>(type: "int", nullable: false),
                    QrCodeMainId = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QrCodeMainId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrLog_QrCodeMain_QrCodeMainId1",
                        column: x => x.QrCodeMainId1,
                        principalTable: "QrCodeMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQrVerification_CustomerId",
                table: "CustomerQrVerification",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQrVerification_QrCodeMainId1",
                table: "CustomerQrVerification",
                column: "QrCodeMainId1");

            migrationBuilder.CreateIndex(
                name: "IX_QrLog_QrCodeMainId1",
                table: "QrLog",
                column: "QrCodeMainId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerQrVerification");

            migrationBuilder.DropTable(
                name: "ParamItem");

            migrationBuilder.DropTable(
                name: "QrCodeDetail");

            migrationBuilder.DropTable(
                name: "QrLog");
        }
    }
}
