using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrDetailId",
                table: "QrCodeMain",
                newName: "DetailTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "QrMainId",
                table: "QrCodeDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "QrCodeAnimalDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrMainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: true),
                    BreedId = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    ColorPattern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccinationStatusId = table.Column<int>(type: "int", nullable: true),
                    IdentificationOrMicrochipNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnershipStatusId = table.Column<int>(type: "int", nullable: true),
                    HealthStatusId = table.Column<int>(type: "int", nullable: true),
                    NutritionStatusId = table.Column<int>(type: "int", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegularMedications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodeAnimalDetail", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrCodeAnimalDetail");

            migrationBuilder.DropColumn(
                name: "QrMainId",
                table: "QrCodeDetail");

            migrationBuilder.RenameColumn(
                name: "DetailTypeId",
                table: "QrCodeMain",
                newName: "QrDetailId");
        }
    }
}
