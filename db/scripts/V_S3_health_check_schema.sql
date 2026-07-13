BEGIN TRANSACTION;
GO

CREATE TABLE [health_check_events] (
    [Id] uniqueidentifier NOT NULL,
    [PetId] uniqueidentifier NULL,
    [Species] int NOT NULL,
    [ImageRef] nvarchar(max) NOT NULL,
    [PreviousImageRef] nvarchar(max) NULL,
    [SubmittedAt] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [AiSummary] nvarchar(max) NULL,
    [DisclaimerShown] bit NOT NULL,
    [ModelVersion] nvarchar(max) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_health_check_events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_health_check_events_PetInfo_PetId] FOREIGN KEY ([PetId]) REFERENCES [PetInfo] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [health_status] (
    [Id] uniqueidentifier NOT NULL,
    [HealthCheckEventId] uniqueidentifier NOT NULL,
    [ConditionName] nvarchar(max) NOT NULL,
    [AffectedArea] nvarchar(max) NULL,
    [Confidence] decimal(5,4) NOT NULL,
    [Severity] int NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_health_status] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_health_status_health_check_events_HealthCheckEventId] FOREIGN KEY ([HealthCheckEventId]) REFERENCES [health_check_events] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_health_check_events_PetId] ON [health_check_events] ([PetId]);
GO

CREATE INDEX [IX_health_status_HealthCheckEventId] ON [health_status] ([HealthCheckEventId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260708100113_AddHealthCheckSchema', N'8.0.8');
GO

COMMIT;
GO

