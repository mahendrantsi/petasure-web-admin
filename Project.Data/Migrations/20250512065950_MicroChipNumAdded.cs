using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class MicroChipNumAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MicrochipNumber",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MicrochipNumber",
                table: "MissingPetLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MicrochipNumber",
                table: "MissingPet",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MicrochipNumber",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "MicrochipNumber",
                table: "MissingPetLogs");

            migrationBuilder.DropColumn(
                name: "MicrochipNumber",
                table: "MissingPet");
        }
    }
}
