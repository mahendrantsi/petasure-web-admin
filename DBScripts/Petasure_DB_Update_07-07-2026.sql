/* =====================================================================
   Petasure - Database Update Script
   Date        : 07-07-2026
   Purpose     : Apply the recent DB changes + required lookup data in ONE run.
   Environments: Safe for dev / staging / production.

   Contents:
     PART 1  Migration 20260512065907_ConfigureForeignKeyRelationships_20260512
             (nullable FK columns, orphan cleanup, indexes, SET NULL FKs)
     PART 2  Migration 20260624140427_AddPetTypeMasterTable
             (drops PetInfo.PetType, adds PetTypeId, creates PetTypeMaster + FK)
     PART 3  Seed PetTypeMaster lookup rows (Dog = 1, Cat = 2) - required by the app.

   Notes:
     * Every step is idempotent - guarded by __EFMigrationsHistory / IF NOT EXISTS,
       so re-running does nothing and will NOT throw "already exists" errors.
     * Existing pets keep PetTypeId = NULL (not back-filled - old PetType values
       were dropped by the migration and cannot be reconstructed). See PART 3.
   ===================================================================== */

-- =====================================================================
-- PART 1: ConfigureForeignKeyRelationships (20260512065907)
-- =====================================================================
BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserProfile]') AND [c].[name] = N'UserId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserProfile] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [UserProfile] ALTER COLUMN [UserId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InAppPurchases]') AND [c].[name] = N'AspnetuserId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [InAppPurchases] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [InAppPurchases] ALTER COLUMN [AspnetuserId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserPasswordToken]') AND [c].[name] = N'UserID');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [UserPasswordToken] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [UserPasswordToken] ALTER COLUMN [UserID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'UserID');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [PetInfo] ALTER COLUMN [UserID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'PetId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [PetId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'MissingPetsID');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [MissingPetsID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'PetId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [PetId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [UserProfile]
    SET [UserId] = NULL
    WHERE [UserId] IS NOT NULL
    AND [UserId] NOT IN (SELECT [Id] FROM [AspNetUsers]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [InAppPurchases]
    SET [AspnetuserId] = NULL
    WHERE [AspnetuserId] IS NOT NULL
    AND [AspnetuserId] NOT IN (SELECT [Id] FROM [AspNetUsers]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [UserPasswordToken]
    SET [UserID] = NULL
    WHERE [UserID] IS NOT NULL
    AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [PetInfo]
    SET [UserID] = NULL
    WHERE [UserID] IS NOT NULL
    AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [MissingPet]
    SET [PetId] = NULL
    WHERE [PetId] IS NOT NULL
    AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [MissingPet]
    SET [FoundBy] = NULL
    WHERE [FoundBy] IS NOT NULL
    AND [FoundBy] NOT IN (SELECT [Id] FROM [AspNetUsers]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [MissingPetLogs]
    SET [MissingPetsID] = NULL
    WHERE [MissingPetsID] IS NOT NULL
    AND [MissingPetsID] NOT IN (SELECT [Id] FROM [MissingPet]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    UPDATE [MissingPetLogs]
    SET [PetId] = NULL
    WHERE [PetId] IS NOT NULL
    AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_UserProfile_UserId] ON [UserProfile] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_InAppPurchases_AspnetuserId] ON [InAppPurchases] ([AspnetuserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_UserPasswordToken_UserID] ON [UserPasswordToken] ([UserID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_PetInfo_UserID] ON [PetInfo] ([UserID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_MissingPets_PetId] ON [MissingPet] ([PetId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_MissingPets_FoundBy] ON [MissingPet] ([FoundBy]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_MissingPetsLogs_MissingPetsID] ON [MissingPetLogs] ([MissingPetsID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_MissingPetsLogs_PetId] ON [MissingPetLogs] ([PetId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    CREATE INDEX [IX_Enquiry_UserId] ON [Enquiry] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [UserProfile] ADD CONSTRAINT [FK_UserProfile_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [InAppPurchases] ADD CONSTRAINT [FK_InAppPurchases_AspNetUsers_AspnetuserId] FOREIGN KEY ([AspnetuserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [UserPasswordToken] ADD CONSTRAINT [FK_UserPasswordToken_AspNetUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [PetInfo] ADD CONSTRAINT [FK_PetInfo_AspNetUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [MissingPet] ADD CONSTRAINT [FK_MissingPets_PetInfo_PetId] FOREIGN KEY ([PetId]) REFERENCES [PetInfo] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [MissingPet] ADD CONSTRAINT [FK_MissingPets_AspNetUsers_FoundBy] FOREIGN KEY ([FoundBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [MissingPetLogs] ADD CONSTRAINT [FK_MissingPetsLogs_MissingPets_MissingPetsID] FOREIGN KEY ([MissingPetsID]) REFERENCES [MissingPet] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [MissingPetLogs] ADD CONSTRAINT [FK_MissingPetsLogs_PetInfo_PetId] FOREIGN KEY ([PetId]) REFERENCES [PetInfo] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    ALTER TABLE [Enquiry] ADD CONSTRAINT [FK_Enquiry_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260512065907_ConfigureForeignKeyRelationships_20260512', N'8.0.8');
END;
GO

COMMIT;
GO

-- =====================================================================
-- PART 2: AddPetTypeMasterTable (20260624140427)
-- =====================================================================
BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'PetType');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [PetInfo] DROP COLUMN [PetType];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [PetTypeId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    CREATE TABLE [PetTypeMaster] (
        [Id] int NOT NULL IDENTITY,
        [TypeName] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_PetTypeMaster] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    CREATE INDEX [IX_PetInfo_PetTypeId] ON [PetInfo] ([PetTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    ALTER TABLE [PetInfo] ADD CONSTRAINT [FK_PetInfo_PetTypeMaster_PetTypeId] FOREIGN KEY ([PetTypeId]) REFERENCES [PetTypeMaster] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624140427_AddPetTypeMasterTable', N'8.0.8');
END;
GO

COMMIT;
GO

-- =====================================================================
-- PART 3: Seed PetTypeMaster lookup rows  (REQUIRED by the app)
--   The app hard-codes: PetTypeId 1 = Dog, 2 = Cat (see PetsController).
-- =====================================================================
BEGIN TRANSACTION;
GO

SET IDENTITY_INSERT [dbo].[PetTypeMaster] ON;

IF NOT EXISTS (SELECT 1 FROM [dbo].[PetTypeMaster] WHERE [Id] = 1)
    INSERT INTO [dbo].[PetTypeMaster] ([Id], [TypeName], [Description]) VALUES (1, N'Dog', N'Canine');

IF NOT EXISTS (SELECT 1 FROM [dbo].[PetTypeMaster] WHERE [Id] = 2)
    INSERT INTO [dbo].[PetTypeMaster] ([Id], [TypeName], [Description]) VALUES (2, N'Cat', N'Feline');

SET IDENTITY_INSERT [dbo].[PetTypeMaster] OFF;

COMMIT;
GO

/* ---------------------------------------------------------------------
   OPTIONAL - back-fill existing pets (they default to PetTypeId = NULL).
   Run only if you know how existing pets should be classified.

   -- Default every existing pet to Dog:
   -- UPDATE [dbo].[PetInfo] SET [PetTypeId] = 1 WHERE [PetTypeId] IS NULL;
   --------------------------------------------------------------------- */
