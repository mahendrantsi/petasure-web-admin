using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthCheckSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "health_check_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Species = table.Column<int>(type: "int", nullable: false),
                    ImageRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousImageRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AiSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerShown = table.Column<bool>(type: "bit", nullable: false),
                    ModelVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_health_check_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_health_check_events_PetInfo_PetId",
                        column: x => x.PetId,
                        principalTable: "PetInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "health_status",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthCheckEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConditionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AffectedArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confidence = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_health_status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_health_status_health_check_events_HealthCheckEventId",
                        column: x => x.HealthCheckEventId,
                        principalTable: "health_check_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_health_check_events_PetId",
                table: "health_check_events",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_health_status_HealthCheckEventId",
                table: "health_status",
                column: "HealthCheckEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "health_status");

            migrationBuilder.DropTable(
                name: "health_check_events");
        }
    }
}
