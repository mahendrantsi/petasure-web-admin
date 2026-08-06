/* =====================================================================
   Petasure - Database Update Script
   Date        : 29-07-2026
   Baseline    : Petasure_DB_Update_07-07-2026.sql
                 (run that script first if this DB has not had it applied)
   Purpose     : Apply every schema change made AFTER 07-07-2026 in ONE run.
   Environments: Safe for dev / staging / production.

   Contents:
     PART 1  Migration 20260708100113_AddHealthCheckSchema
             (creates health_check_events + health_status and their indexes/FKs)
     PART 2  Migration 20260715070141_AddPetRecognitionSchema
             (creates pet_images, pet_scans, recognition_errors;
              adds health_check_events.PetScanId + FK/index)

   Notes:
     * Every step is idempotent - guarded by __EFMigrationsHistory / IF NOT EXISTS,
       so re-running does nothing and will NOT throw "already exists" errors.
     * No data migration or back-fill is needed: all objects below are new.
     * Requires the PetInfo table to exist (created by earlier migrations).
   ===================================================================== */

-- =====================================================================
-- PART 1: AddHealthCheckSchema (20260708100113)
-- =====================================================================
BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260708100113_AddHealthCheckSchema'
)
AND OBJECT_ID(N'[dbo].[health_check_events]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[health_check_events] (
        [Id]               uniqueidentifier NOT NULL,
        [PetId]            uniqueidentifier NULL,
        [Species]          int              NOT NULL,
        [ImageRef]         nvarchar(max)    NOT NULL,
        [PreviousImageRef] nvarchar(max)    NULL,
        [SubmittedAt]      datetime2        NOT NULL,
        [Status]           int              NOT NULL,
        [AiSummary]        nvarchar(max)    NULL,
        [DisclaimerShown]  bit              NOT NULL,
        [ModelVersion]     nvarchar(max)    NULL,
        [CreatedOn]        datetime2        NOT NULL,
        [CreatedBy]        uniqueidentifier NOT NULL,
        CONSTRAINT [PK_health_check_events] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_health_check_events_PetInfo_PetId] FOREIGN KEY ([PetId])
            REFERENCES [dbo].[PetInfo] ([Id]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260708100113_AddHealthCheckSchema'
)
AND OBJECT_ID(N'[dbo].[health_status]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[health_status] (
        [Id]                 uniqueidentifier NOT NULL,
        [HealthCheckEventId] uniqueidentifier NOT NULL,
        [ConditionName]      nvarchar(max)    NOT NULL,
        [AffectedArea]       nvarchar(max)    NULL,
        [Confidence]         decimal(5,4)     NOT NULL,
        [Severity]           int              NOT NULL,
        [CreatedOn]          datetime2        NOT NULL,
        [CreatedBy]          uniqueidentifier NOT NULL,
        CONSTRAINT [PK_health_status] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_health_status_health_check_events_HealthCheckEventId]
            FOREIGN KEY ([HealthCheckEventId])
            REFERENCES [dbo].[health_check_events] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_health_check_events_PetId'
               AND [object_id] = OBJECT_ID(N'[dbo].[health_check_events]'))
    CREATE INDEX [IX_health_check_events_PetId] ON [dbo].[health_check_events] ([PetId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_health_status_HealthCheckEventId'
               AND [object_id] = OBJECT_ID(N'[dbo].[health_status]'))
    CREATE INDEX [IX_health_status_HealthCheckEventId] ON [dbo].[health_status] ([HealthCheckEventId]);
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260708100113_AddHealthCheckSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260708100113_AddHealthCheckSchema', N'8.0.8');
END;
GO

COMMIT;
GO

-- =====================================================================
-- PART 2: AddPetRecognitionSchema (20260715070141)
--   pet_images must be created before pet_scans (FK dependency),
--   and pet_scans before recognition_errors / the health_check_events FK.
-- =====================================================================
BEGIN TRANSACTION;
GO

IF OBJECT_ID(N'[dbo].[pet_images]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[pet_images] (
        [Id]               uniqueidentifier NOT NULL,
        [PetId]            uniqueidentifier NULL,
        [ImageKind]        int              NOT NULL,
        [StoragePath]      nvarchar(max)    NOT NULL,
        [OriginalFileName] nvarchar(max)    NULL,
        [ContentType]      nvarchar(max)    NULL,
        [FileSizeBytes]    bigint           NULL,
        [CreatedOn]        datetime2        NOT NULL,
        [CreatedBy]        uniqueidentifier NOT NULL,
        CONSTRAINT [PK_pet_images] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_pet_images_PetInfo_PetId] FOREIGN KEY ([PetId])
            REFERENCES [dbo].[PetInfo] ([Id]) ON DELETE SET NULL
    );
END;
GO

IF OBJECT_ID(N'[dbo].[pet_scans]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[pet_scans] (
        [Id]                    uniqueidentifier NOT NULL,
        [PetId]                 uniqueidentifier NULL,
        [ScanType]              int              NOT NULL,
        [Species]               int              NOT NULL,
        [PrimaryImageId]        uniqueidentifier NULL,
        [SecondaryImageId]      uniqueidentifier NULL,
        [RouteDecision]         nvarchar(max)    NULL,
        [ClassifierLabel]       nvarchar(max)    NULL,
        [ClassifierConfidence]  decimal(5,4)     NULL,
        [ClassifierDogScore]    decimal(5,4)     NULL,
        [ClassifierCatScore]    decimal(5,4)     NULL,
        [MatchResult]           nvarchar(max)    NULL,
        [MatchConfidence]       decimal(9,6)     NULL,
        [MatchedDsId]           nvarchar(max)    NULL,
        [IsBlurRejected]        bit              NOT NULL,
        [IsNoseDetected]        bit              NULL,
        [AiResponseRaw]         nvarchar(max)    NULL,
        [AiStatusCode]          int              NULL,
        [AiRequestDurationMs]   int              NULL,
        [Status]                int              NOT NULL,
        [Notes]                 nvarchar(max)    NULL,
        [CreatedOn]             datetime2        NOT NULL,
        [CreatedBy]             uniqueidentifier NOT NULL,
        CONSTRAINT [PK_pet_scans] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_pet_scans_PetInfo_PetId] FOREIGN KEY ([PetId])
            REFERENCES [dbo].[PetInfo] ([Id]) ON DELETE SET NULL,
        -- RESTRICT (NO ACTION): an image row cannot be deleted while a scan references it.
        CONSTRAINT [FK_pet_scans_pet_images_PrimaryImageId] FOREIGN KEY ([PrimaryImageId])
            REFERENCES [dbo].[pet_images] ([Id]),
        CONSTRAINT [FK_pet_scans_pet_images_SecondaryImageId] FOREIGN KEY ([SecondaryImageId])
            REFERENCES [dbo].[pet_images] ([Id])
    );
END;
GO

IF OBJECT_ID(N'[dbo].[recognition_errors]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[recognition_errors] (
        [Id]                 uniqueidentifier NOT NULL,
        [PetScanId]          uniqueidentifier NOT NULL,
        [ErrorStage]         int              NOT NULL,
        [ErrorMessage]       nvarchar(max)    NOT NULL,
        [StatusCodeReturned] int              NULL,
        [CreatedOn]          datetime2        NOT NULL,
        [CreatedBy]          uniqueidentifier NOT NULL,
        CONSTRAINT [PK_recognition_errors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_recognition_errors_pet_scans_PetScanId] FOREIGN KEY ([PetScanId])
            REFERENCES [dbo].[pet_scans] ([Id]) ON DELETE CASCADE
    );
END;
GO

-- Link an ill-health check back to the recognition scan that produced it.
IF COL_LENGTH(N'[dbo].[health_check_events]', N'PetScanId') IS NULL
    ALTER TABLE [dbo].[health_check_events] ADD [PetScanId] uniqueidentifier NULL;
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_health_check_events_PetScanId'
               AND [object_id] = OBJECT_ID(N'[dbo].[health_check_events]'))
    CREATE INDEX [IX_health_check_events_PetScanId] ON [dbo].[health_check_events] ([PetScanId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_pet_images_PetId'
               AND [object_id] = OBJECT_ID(N'[dbo].[pet_images]'))
    CREATE INDEX [IX_pet_images_PetId] ON [dbo].[pet_images] ([PetId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_pet_scans_PetId'
               AND [object_id] = OBJECT_ID(N'[dbo].[pet_scans]'))
    CREATE INDEX [IX_pet_scans_PetId] ON [dbo].[pet_scans] ([PetId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_pet_scans_PrimaryImageId'
               AND [object_id] = OBJECT_ID(N'[dbo].[pet_scans]'))
    CREATE INDEX [IX_pet_scans_PrimaryImageId] ON [dbo].[pet_scans] ([PrimaryImageId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_pet_scans_SecondaryImageId'
               AND [object_id] = OBJECT_ID(N'[dbo].[pet_scans]'))
    CREATE INDEX [IX_pet_scans_SecondaryImageId] ON [dbo].[pet_scans] ([SecondaryImageId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[indexes] WHERE [name] = N'IX_recognition_errors_PetScanId'
               AND [object_id] = OBJECT_ID(N'[dbo].[recognition_errors]'))
    CREATE INDEX [IX_recognition_errors_PetScanId] ON [dbo].[recognition_errors] ([PetScanId]);
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[foreign_keys]
               WHERE [name] = N'FK_health_check_events_pet_scans_PetScanId')
    ALTER TABLE [dbo].[health_check_events]
        ADD CONSTRAINT [FK_health_check_events_pet_scans_PetScanId] FOREIGN KEY ([PetScanId])
        REFERENCES [dbo].[pet_scans] ([Id]) ON DELETE SET NULL;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260715070141_AddPetRecognitionSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260715070141_AddPetRecognitionSchema', N'8.0.8');
END;
GO

COMMIT;
GO

/* ---------------------------------------------------------------------
   VERIFICATION - run after the script to confirm the objects exist.

   SELECT [MigrationId] FROM [__EFMigrationsHistory]
   WHERE [MigrationId] IN (N'20260708100113_AddHealthCheckSchema',
                           N'20260715070141_AddPetRecognitionSchema');

   SELECT name FROM sys.tables
   WHERE name IN ('health_check_events','health_status',
                  'pet_images','pet_scans','recognition_errors');
   --------------------------------------------------------------------- */
