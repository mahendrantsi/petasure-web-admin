using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPetRecognitionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PetScanId",
                table: "health_check_events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "pet_images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageKind = table.Column<int>(type: "int", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pet_images_PetInfo_PetId",
                        column: x => x.PetId,
                        principalTable: "PetInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "pet_scans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScanType = table.Column<int>(type: "int", nullable: false),
                    Species = table.Column<int>(type: "int", nullable: false),
                    PrimaryImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondaryImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RouteDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassifierLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassifierConfidence = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    ClassifierDogScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    ClassifierCatScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    MatchResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchConfidence = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    MatchedDsId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlurRejected = table.Column<bool>(type: "bit", nullable: false),
                    IsNoseDetected = table.Column<bool>(type: "bit", nullable: true),
                    AiResponseRaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiStatusCode = table.Column<int>(type: "int", nullable: true),
                    AiRequestDurationMs = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_scans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pet_scans_PetInfo_PetId",
                        column: x => x.PetId,
                        principalTable: "PetInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_pet_scans_pet_images_PrimaryImageId",
                        column: x => x.PrimaryImageId,
                        principalTable: "pet_images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pet_scans_pet_images_SecondaryImageId",
                        column: x => x.SecondaryImageId,
                        principalTable: "pet_images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recognition_errors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetScanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorStage = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCodeReturned = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recognition_errors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recognition_errors_pet_scans_PetScanId",
                        column: x => x.PetScanId,
                        principalTable: "pet_scans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_health_check_events_PetScanId",
                table: "health_check_events",
                column: "PetScanId");

            migrationBuilder.CreateIndex(
                name: "IX_pet_images_PetId",
                table: "pet_images",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_pet_scans_PetId",
                table: "pet_scans",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_pet_scans_PrimaryImageId",
                table: "pet_scans",
                column: "PrimaryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_pet_scans_SecondaryImageId",
                table: "pet_scans",
                column: "SecondaryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_recognition_errors_PetScanId",
                table: "recognition_errors",
                column: "PetScanId");

            migrationBuilder.AddForeignKey(
                name: "FK_health_check_events_pet_scans_PetScanId",
                table: "health_check_events",
                column: "PetScanId",
                principalTable: "pet_scans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_health_check_events_pet_scans_PetScanId",
                table: "health_check_events");

            migrationBuilder.DropTable(
                name: "recognition_errors");

            migrationBuilder.DropTable(
                name: "pet_scans");

            migrationBuilder.DropTable(
                name: "pet_images");

            migrationBuilder.DropIndex(
                name: "IX_health_check_events_PetScanId",
                table: "health_check_events");

            migrationBuilder.DropColumn(
                name: "PetScanId",
                table: "health_check_events");
        }
    }
}
