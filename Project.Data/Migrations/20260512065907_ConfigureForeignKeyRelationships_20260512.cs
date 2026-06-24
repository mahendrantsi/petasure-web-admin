using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureForeignKeyRelationships_20260512 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: ALTER columns to be nullable before adding SET NULL FKs
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserProfile",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AspnetuserId",
                table: "InAppPurchases",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserPasswordToken",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "PetInfo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetId",
                table: "MissingPet",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MissingPetsID",
                table: "MissingPetLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetId",
                table: "MissingPetLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Step 2: Clean up orphaned FK values that don't exist in parent tables
            migrationBuilder.Sql(@"
                UPDATE [UserProfile]
                SET [UserId] = NULL
                WHERE [UserId] IS NOT NULL
                AND [UserId] NOT IN (SELECT [Id] FROM [AspNetUsers])
            ");

            migrationBuilder.Sql(@"
                UPDATE [InAppPurchases]
                SET [AspnetuserId] = NULL
                WHERE [AspnetuserId] IS NOT NULL
                AND [AspnetuserId] NOT IN (SELECT [Id] FROM [AspNetUsers])
            ");

            migrationBuilder.Sql(@"
                UPDATE [UserPasswordToken]
                SET [UserID] = NULL
                WHERE [UserID] IS NOT NULL
                AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers])
            ");

            migrationBuilder.Sql(@"
                UPDATE [PetInfo]
                SET [UserID] = NULL
                WHERE [UserID] IS NOT NULL
                AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers])
            ");

            migrationBuilder.Sql(@"
                UPDATE [MissingPet]
                SET [PetId] = NULL
                WHERE [PetId] IS NOT NULL
                AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo])
            ");

            migrationBuilder.Sql(@"
                UPDATE [MissingPet]
                SET [FoundBy] = NULL
                WHERE [FoundBy] IS NOT NULL
                AND [FoundBy] NOT IN (SELECT [Id] FROM [AspNetUsers])
            ");

            migrationBuilder.Sql(@"
                UPDATE [MissingPetLogs]
                SET [MissingPetsID] = NULL
                WHERE [MissingPetsID] IS NOT NULL
                AND [MissingPetsID] NOT IN (SELECT [Id] FROM [MissingPet])
            ");

            migrationBuilder.Sql(@"
                UPDATE [MissingPetLogs]
                SET [PetId] = NULL
                WHERE [PetId] IS NOT NULL
                AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo])
            ");

            // Step 3: Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfile",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InAppPurchases_AspnetuserId",
                table: "InAppPurchases",
                column: "AspnetuserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPasswordToken_UserID",
                table: "UserPasswordToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PetInfo_UserID",
                table: "PetInfo",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MissingPets_PetId",
                table: "MissingPet",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_MissingPets_FoundBy",
                table: "MissingPet",
                column: "FoundBy");

            migrationBuilder.CreateIndex(
                name: "IX_MissingPetsLogs_MissingPetsID",
                table: "MissingPetLogs",
                column: "MissingPetsID");

            migrationBuilder.CreateIndex(
                name: "IX_MissingPetsLogs_PetId",
                table: "MissingPetLogs",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiry_UserId",
                table: "Enquiry",
                column: "UserId");

            // Step 4: Add Foreign Key Constraints with SetNull behavior
            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_AspNetUsers_UserId",
                table: "UserProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InAppPurchases_AspNetUsers_AspnetuserId",
                table: "InAppPurchases",
                column: "AspnetuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPasswordToken_AspNetUsers_UserID",
                table: "UserPasswordToken",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PetInfo_AspNetUsers_UserID",
                table: "PetInfo",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MissingPets_PetInfo_PetId",
                table: "MissingPet",
                column: "PetId",
                principalTable: "PetInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MissingPets_AspNetUsers_FoundBy",
                table: "MissingPet",
                column: "FoundBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MissingPetsLogs_MissingPets_MissingPetsID",
                table: "MissingPetLogs",
                column: "MissingPetsID",
                principalTable: "MissingPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MissingPetsLogs_PetInfo_PetId",
                table: "MissingPetLogs",
                column: "PetId",
                principalTable: "PetInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiry_AspNetUsers_UserId",
                table: "Enquiry",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_AspNetUsers_UserId",
                table: "UserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_InAppPurchases_AspNetUsers_AspnetuserId",
                table: "InAppPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPasswordToken_AspNetUsers_UserID",
                table: "UserPasswordToken");

            migrationBuilder.DropForeignKey(
                name: "FK_PetInfo_AspNetUsers_UserID",
                table: "PetInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MissingPets_PetInfo_PetId",
                table: "MissingPet");

            migrationBuilder.DropForeignKey(
                name: "FK_MissingPets_AspNetUsers_FoundBy",
                table: "MissingPet");

            migrationBuilder.DropForeignKey(
                name: "FK_MissingPetsLogs_MissingPets_MissingPetsID",
                table: "MissingPetLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MissingPetsLogs_PetInfo_PetId",
                table: "MissingPetLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Enquiry_AspNetUsers_UserId",
                table: "Enquiry");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_InAppPurchases_AspnetuserId",
                table: "InAppPurchases");

            migrationBuilder.DropIndex(
                name: "IX_UserPasswordToken_UserID",
                table: "UserPasswordToken");

            migrationBuilder.DropIndex(
                name: "IX_PetInfo_UserID",
                table: "PetInfo");

            migrationBuilder.DropIndex(
                name: "IX_MissingPets_PetId",
                table: "MissingPet");

            migrationBuilder.DropIndex(
                name: "IX_MissingPets_FoundBy",
                table: "MissingPet");

            migrationBuilder.DropIndex(
                name: "IX_MissingPetsLogs_MissingPetsID",
                table: "MissingPetLogs");

            migrationBuilder.DropIndex(
                name: "IX_MissingPetsLogs_PetId",
                table: "MissingPetLogs");

            migrationBuilder.DropIndex(
                name: "IX_Enquiry_UserId",
                table: "Enquiry");

            // Restore columns to NOT NULL in Down()
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserProfile",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AspnetuserId",
                table: "InAppPurchases",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "UserPasswordToken",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "PetInfo",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetId",
                table: "MissingPet",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MissingPetsID",
                table: "MissingPetLogs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetId",
                table: "MissingPetLogs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
