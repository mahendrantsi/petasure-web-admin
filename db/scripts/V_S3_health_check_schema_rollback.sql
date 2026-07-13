BEGIN TRANSACTION;
GO

DROP TABLE [health_status];
GO

DROP TABLE [health_check_events];
GO

DELETE FROM [__EFMigrationsHistory]
WHERE [MigrationId] = N'20260708100113_AddHealthCheckSchema';
GO

COMMIT;
GO

