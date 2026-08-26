using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSideViewImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeftViewImagePath",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightViewImagePath",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopViewImagePath",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftViewImagePath",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "RightViewImagePath",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "TopViewImagePath",
                table: "PetInfo");
        }
    }
}
