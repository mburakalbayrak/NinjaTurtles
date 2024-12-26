using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NinjaTurtles.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedId",
                table: "QrCodeAnimalDetail");

            migrationBuilder.DropColumn(
                name: "SpeciesId",
                table: "QrCodeAnimalDetail");

            migrationBuilder.AddColumn<string>(
                name: "BreedName",
                table: "QrCodeAnimalDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpeciesName",
                table: "QrCodeAnimalDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedName",
                table: "QrCodeAnimalDetail");

            migrationBuilder.DropColumn(
                name: "SpeciesName",
                table: "QrCodeAnimalDetail");

            migrationBuilder.AddColumn<int>(
                name: "BreedId",
                table: "QrCodeAnimalDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeciesId",
                table: "QrCodeAnimalDetail",
                type: "int",
                nullable: true);
        }
    }
}
