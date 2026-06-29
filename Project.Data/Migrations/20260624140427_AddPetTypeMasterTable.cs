using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPetTypeMasterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PetType",
                table: "PetInfo");

            migrationBuilder.AddColumn<int>(
                name: "PetTypeId",
                table: "PetInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PetTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetInfo_PetTypeId",
                table: "PetInfo",
                column: "PetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetInfo_PetTypeMaster_PetTypeId",
                table: "PetInfo",
                column: "PetTypeId",
                principalTable: "PetTypeMaster",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetInfo_PetTypeMaster_PetTypeId",
                table: "PetInfo");

            migrationBuilder.DropTable(
                name: "PetTypeMaster");

            migrationBuilder.DropIndex(
                name: "IX_PetInfo_PetTypeId",
                table: "PetInfo");

            migrationBuilder.DropColumn(
                name: "PetTypeId",
                table: "PetInfo");

            migrationBuilder.AddColumn<int>(
                name: "PetType",
                table: "PetInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
