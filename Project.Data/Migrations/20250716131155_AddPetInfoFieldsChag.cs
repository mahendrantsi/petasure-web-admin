using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPetInfoFieldsChag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BreedDescription",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Breeder",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "PetInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IssuingAuthority",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenceNumber",
                table: "PetInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("4b79e105-758c-4fbc-9333-4be0b74bc3f8"),
                column: "Address",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedDescription",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "Breeder",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "IssuingAuthority",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "LicenceNumber",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");
        }
    }
}
