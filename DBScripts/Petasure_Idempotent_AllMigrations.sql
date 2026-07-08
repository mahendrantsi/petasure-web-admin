IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Discriminator] nvarchar(21) NOT NULL,
        [IsActive] bit NULL,
        [CreatedOn] datetime2 NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(250) NULL,
        [LastName] nvarchar(250) NULL,
        [IsActive] bit NULL,
        [ParentUserID] uniqueidentifier NULL,
        [IsDeleted] bit NULL,
        [IsDeviceConnected] bit NULL,
        [CreatedOn] datetime2 NOT NULL,
        [PhoneNumberConfirmedOn] datetime2 NULL,
        [RefreshToken] nvarchar(max) NULL,
        [RefreshTokenExpiryTime] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(450) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [ContentInfo] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [IsActive] bit NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Url] nvarchar(max) NULL,
        [ModifiedOn] datetime2 NULL,
        [ModifiedBy] uniqueidentifier NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ContentInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [EmailLog] (
        [Id] bigint NOT NULL IDENTITY,
        [FromMail] nvarchar(max) NOT NULL,
        [ToMail] nvarchar(max) NOT NULL,
        [CcMail] nvarchar(max) NULL,
        [BccMail] nvarchar(max) NULL,
        [Subject] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [MailStatus] nvarchar(max) NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [SendResult] nvarchar(max) NULL,
        [SendResultId] nvarchar(max) NULL,
        CONSTRAINT [PK_EmailLog] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [Enquiry] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [PhoneNo] nvarchar(max) NULL,
        [Email] nvarchar(max) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [ReadBy] int NULL,
        [EnquiryType] int NOT NULL,
        [UserId] uniqueidentifier NULL,
        [ReadOn] datetime2 NULL,
        [Status] int NOT NULL,
        [EnquiryCode] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Enquiry] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [ExceptionLogger] (
        [Id] uniqueidentifier NOT NULL,
        [Exception] nvarchar(max) NULL,
        [InnerException] nvarchar(max) NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ExceptionLogger] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [FAQ] (
        [Id] uniqueidentifier NOT NULL,
        [Question] nvarchar(max) NULL,
        [Answer] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [Order] int NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_FAQ] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [Integration] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Status] nvarchar(max) NULL,
        [Image] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Integration] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [MissingPet] (
        [Id] uniqueidentifier NOT NULL,
        [PetId] uniqueidentifier NOT NULL,
        [Address] nvarchar(500) NULL,
        [Description] nvarchar(500) NULL,
        [Lat] decimal(18,2) NOT NULL,
        [Long] decimal(18,2) NOT NULL,
        [LostDate] datetime2 NOT NULL,
        [FoundAddress] nvarchar(500) NULL,
        [FoundLat] decimal(18,2) NULL,
        [FoundLong] decimal(18,2) NULL,
        [FoundBy] uniqueidentifier NULL,
        [Status] int NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MissingPet] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [MissingPetLogs] (
        [Id] uniqueidentifier NOT NULL,
        [MissingPetsID] uniqueidentifier NOT NULL,
        [PetId] uniqueidentifier NOT NULL,
        [Address] nvarchar(500) NULL,
        [Description] nvarchar(500) NULL,
        [Lat] decimal(18,2) NOT NULL,
        [Long] decimal(18,2) NOT NULL,
        [LostDate] datetime2 NOT NULL,
        [FoundAddress] nvarchar(500) NULL,
        [FoundLat] decimal(18,2) NULL,
        [FoundLong] decimal(18,2) NULL,
        [FoundBy] uniqueidentifier NULL,
        [Status] int NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MissingPetLogs] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [PetInfo] (
        [Id] uniqueidentifier NOT NULL,
        [PName] nvarchar(100) NOT NULL,
        [PSex] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [ContactNumber] nvarchar(max) NULL,
        [UserID] uniqueidentifier NOT NULL,
        [IsMissing] bit NOT NULL,
        [PDataScienceId] nvarchar(max) NULL,
        [IsDelete] bit NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_PetInfo] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [Settings] (
        [Id] uniqueidentifier NOT NULL,
        [IsEmailConfirmed] bit NOT NULL,
        [IsPhoneVerificationRequired] bit NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Settings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [tblCountry] (
        [Id] bigint NOT NULL IDENTITY,
        [CountryName] nvarchar(max) NULL,
        [ShortCode] nvarchar(max) NULL,
        [Code] nvarchar(max) NULL,
        [DialCode] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_tblCountry] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [UserPasswordToken] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NULL,
        [UserID] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_UserPasswordToken] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [UserProfile] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [FCMToken] nvarchar(max) NULL,
        [DeviceType] nvarchar(max) NULL,
        CONSTRAINT [PK_UserProfile] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedOn', N'Discriminator', N'IsActive', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [CreatedOn], [Discriminator], [IsActive], [Name], [NormalizedName])
    VALUES (''0b9f1b81-5c09-4237-bcc9-0390044ebf0d'', NULL, ''2024-11-10T00:00:00.0000000'', N''DerivedIdentityRole'', CAST(1 AS bit), N''User'', N''USER''),
    (''1f729636-ebdd-42a1-8633-a43de9a5668b'', NULL, ''2024-11-10T00:00:00.0000000'', N''DerivedIdentityRole'', CAST(1 AS bit), N''AnonymousUser'', N''ANONYMOUSUSER''),
    (''6ff06e0d-3e8d-4f9e-bbe9-7ef907bff3a8'', NULL, ''2024-11-10T00:00:00.0000000'', N''DerivedIdentityRole'', CAST(1 AS bit), N''SecondayUser'', N''SECONDAYUSER''),
    (''d5c13504-9424-4e06-abe9-a74ccbb5c056'', NULL, ''2024-11-10T00:00:00.0000000'', N''DerivedIdentityRole'', CAST(1 AS bit), N''SubUser'', N''SUBUSER''),
    (''f1213165-fe5f-4750-affc-1b3136fd613b'', NULL, ''2024-11-10T00:00:00.0000000'', N''DerivedIdentityRole'', CAST(1 AS bit), N''Admin'', N''ADMIN'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedOn', N'Discriminator', N'IsActive', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'IsActive', N'IsDeleted', N'IsDeviceConnected', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'ParentUserID', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'PhoneNumberConfirmedOn', N'RefreshToken', N'RefreshTokenExpiryTime', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] ON;
    EXEC(N'INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedOn], [Email], [EmailConfirmed], [FirstName], [IsActive], [IsDeleted], [IsDeviceConnected], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [ParentUserID], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [PhoneNumberConfirmedOn], [RefreshToken], [RefreshTokenExpiryTime], [SecurityStamp], [TwoFactorEnabled], [UserName])
    VALUES (''4b79e105-758c-4fbc-9333-4be0b74bc3f8'', 0, N''9ca8abe8-a776-4f8b-9a6e-795ed3407f1a'', ''2024-11-10T00:00:00.0000000'', N''dsadmin@yopmail.com'', CAST(1 AS bit), N''Dotsquare'', CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit), N''Admin'', CAST(0 AS bit), NULL, N''DSADMIN@YOPMAIL.COM'', N''ADMIN'', NULL, N''APz2fRvKE8u+ZBkGL+e2crbWGxSPiIPW/QqUnZiPGizQcA5FNToy/ED5JYV7+ujpiQ=='', N''7037353635'', CAST(1 AS bit), NULL, NULL, ''2030-11-10T00:00:00.0000000'', N''BRYBDSPPOB5WW7REAMP2I55HBJGGO3VU'', CAST(0 AS bit), N''admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'IsActive', N'IsDeleted', N'IsDeviceConnected', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'ParentUserID', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'PhoneNumberConfirmedOn', N'RefreshToken', N'RefreshTokenExpiryTime', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedOn', N'IsEmailConfirmed', N'IsPhoneVerificationRequired') AND [object_id] = OBJECT_ID(N'[Settings]'))
        SET IDENTITY_INSERT [Settings] ON;
    EXEC(N'INSERT INTO [Settings] ([Id], [CreatedBy], [CreatedOn], [IsEmailConfirmed], [IsPhoneVerificationRequired])
    VALUES (''b7525cb0-0ec3-4146-a0fb-e80c7902908a'', ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'', ''2024-11-10T00:00:00.0000000'', CAST(1 AS bit), CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'CreatedOn', N'IsEmailConfirmed', N'IsPhoneVerificationRequired') AND [object_id] = OBJECT_ID(N'[Settings]'))
        SET IDENTITY_INSERT [Settings] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] ON;
    EXEC(N'INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
    VALUES (''f1213165-fe5f-4750-affc-1b3136fd613b'', ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_AspNetUsers_Email] ON [AspNetUsers] ([Email]) WHERE [Email] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_AspNetUsers_PhoneNumber] ON [AspNetUsers] ([PhoneNumber]) WHERE [PhoneNumber] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241218091012_initalMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241218091012_initalMigration', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219133841_shopifyid'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContentInfo]') AND [c].[name] = N'ModifiedBy');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ContentInfo] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ContentInfo] ALTER COLUMN [ModifiedBy] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219133841_shopifyid'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ShopifyId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219133841_shopifyid'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ShopifyId] = 0
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241219133841_shopifyid'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241219133841_shopifyid', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241225064800_shopifyres'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ShopifyResponse] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241225064800_shopifyres'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ShopifyResponse] = NULL
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241225064800_shopifyres'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241225064800_shopifyres', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241227061427_imagepath'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [FullBodyImagePath] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241227061427_imagepath'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [NoseImagePath] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241227061427_imagepath'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ImagePath] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241227061427_imagepath'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ImagePath] = NULL
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241227061427_imagepath'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241227061427_imagepath', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250102060206_enquirytable'
)
BEGIN
    CREATE TABLE [Enquiry] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [PhoneNo] nvarchar(max) NULL,
        [Email] nvarchar(max) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [ReadBy] int NULL,
        [EnquiryType] int NOT NULL,
        [UserId] uniqueidentifier NULL,
        [ReadOn] datetime2 NULL,
        [Status] int NOT NULL,
        [EnquiryCode] uniqueidentifier NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Enquiry] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250102060206_enquirytable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250102060206_enquirytable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250102061809_enquirytable_subject'
)
BEGIN
    ALTER TABLE [Enquiry] ADD [Subject] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250102061809_enquirytable_subject'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250102061809_enquirytable_subject', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104111256_latlonginpetinfo'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [Lat] decimal(18,2) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104111256_latlonginpetinfo'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [Long] decimal(18,2) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104111256_latlonginpetinfo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250104111256_latlonginpetinfo', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'Long');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [PetInfo] ALTER COLUMN [Long] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'Lat');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [PetInfo] ALTER COLUMN [Lat] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'Long');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [Long] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'Lat');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [Lat] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'FoundLong');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [FoundLong] decimal(18,10) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'FoundLat');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [FoundLat] decimal(18,10) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'Long');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [Long] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'Lat');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [Lat] decimal(18,10) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'FoundLong');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [FoundLong] decimal(18,10) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'FoundLat');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [FoundLat] decimal(18,10) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250104112514_latlongdecimalprecition'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250104112514_latlongdecimalprecition', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250108111643_subscriptiontbl'
)
BEGIN
    CREATE TABLE [Subscriptions] (
        [Id] bigint NOT NULL IDENTITY,
        [SubscriptionId] bigint NOT NULL,
        [CancellationReason] nvarchar(max) NULL,
        [CancellationReasonComments] nvarchar(max) NULL,
        [ChargeDelay] nvarchar(max) NULL,
        [CancelledOn] datetime2 NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [ChargeInvervalFrequency] int NOT NULL,
        [CustomerId] nvarchar(max) NULL,
        [NextChargeScheduleOn] datetime2 NOT NULL,
        [OrderInvervalFrequency] int NOT NULL,
        [OrderInvervalUnit] nvarchar(max) NULL,
        [ProductTitle] nvarchar(max) NULL,
        [Quantity] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Status] nvarchar(max) NULL,
        [UpdatedOn] datetime2 NOT NULL,
        [VariantTitle] nvarchar(max) NULL,
        [RechargeProductId] nvarchar(max) NULL,
        [ShopifyProductId] nvarchar(max) NULL,
        CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250108111643_subscriptiontbl'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250108111643_subscriptiontbl', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109090217_dtnullable'
)
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Subscriptions]') AND [c].[name] = N'UpdatedOn');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Subscriptions] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Subscriptions] ALTER COLUMN [UpdatedOn] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109090217_dtnullable'
)
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Subscriptions]') AND [c].[name] = N'NextChargeScheduleOn');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Subscriptions] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Subscriptions] ALTER COLUMN [NextChargeScheduleOn] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109090217_dtnullable'
)
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Subscriptions]') AND [c].[name] = N'CancelledOn');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Subscriptions] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Subscriptions] ALTER COLUMN [CancelledOn] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109090217_dtnullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250109090217_dtnullable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250128134120_custidtoint'
)
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Subscriptions]') AND [c].[name] = N'CustomerId');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Subscriptions] DROP CONSTRAINT [' + @var14 + '];');
    EXEC(N'UPDATE [Subscriptions] SET [CustomerId] = 0 WHERE [CustomerId] IS NULL');
    ALTER TABLE [Subscriptions] ALTER COLUMN [CustomerId] int NOT NULL;
    ALTER TABLE [Subscriptions] ADD DEFAULT 0 FOR [CustomerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250128134120_custidtoint'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250128134120_custidtoint', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250218042543_inapppurchases'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [UserType] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250218042543_inapppurchases'
)
BEGIN
    CREATE TABLE [InAppPurchases] (
        [Id] uniqueidentifier NOT NULL,
        [AspnetuserId] uniqueidentifier NOT NULL,
        [TransactionId] nvarchar(max) NULL,
        [ProductId] nvarchar(max) NULL,
        [TransactionDate] datetime2 NOT NULL,
        [TransactionReceipt] nvarchar(max) NULL,
        [PurchaseToken] nvarchar(max) NULL,
        [Acknowledged] bit NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_InAppPurchases] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250218042543_inapppurchases'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [UserType] = 0
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250218042543_inapppurchases'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250218042543_inapppurchases', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250219065641_inapp_act_title'
)
BEGIN
    ALTER TABLE [InAppPurchases] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250219065641_inapp_act_title'
)
BEGIN
    ALTER TABLE [InAppPurchases] ADD [ProductTitle] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250219065641_inapp_act_title'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250219065641_inapp_act_title', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250220113725_expiredate'
)
BEGIN
    ALTER TABLE [InAppPurchases] ADD [ExpireDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250220113725_expiredate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250220113725_expiredate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512065950_MicroChipNumAdded'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [MicrochipNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512065950_MicroChipNumAdded'
)
BEGIN
    ALTER TABLE [MissingPetLogs] ADD [MicrochipNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512065950_MicroChipNumAdded'
)
BEGIN
    ALTER TABLE [MissingPet] ADD [MicrochipNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250512065950_MicroChipNumAdded'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250512065950_MicroChipNumAdded', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [BreedDescription] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [Breeder] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [Colour] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [DateOfBirth] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [IssuingAuthority] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [PetInfo] ADD [LicenceNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Address] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [Address] = NULL
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250716131155_AddPetInfoFieldsChag'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250716131155_AddPetInfoFieldsChag', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250717052710_AddAddressField'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250717052710_AddAddressField', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251027052746_userProfileFieldUpdate'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [IssuingAuthority] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251027052746_userProfileFieldUpdate'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [LicenseNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251027052746_userProfileFieldUpdate'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [IssuingAuthority] = NULL, [LicenseNumber] = NULL
    WHERE [Id] = ''4b79e105-758c-4fbc-9333-4be0b74bc3f8'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251027052746_userProfileFieldUpdate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251027052746_userProfileFieldUpdate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserProfile]') AND [c].[name] = N'UserId');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [UserProfile] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [UserProfile] ALTER COLUMN [UserId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InAppPurchases]') AND [c].[name] = N'AspnetuserId');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [InAppPurchases] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [InAppPurchases] ALTER COLUMN [AspnetuserId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserPasswordToken]') AND [c].[name] = N'UserID');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [UserPasswordToken] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [UserPasswordToken] ALTER COLUMN [UserID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'UserID');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [PetInfo] ALTER COLUMN [UserID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPet]') AND [c].[name] = N'PetId');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [MissingPet] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [MissingPet] ALTER COLUMN [PetId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'MissingPetsID');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [MissingPetLogs] ALTER COLUMN [MissingPetsID] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512065907_ConfigureForeignKeyRelationships_20260512'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MissingPetLogs]') AND [c].[name] = N'PetId');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [MissingPetLogs] DROP CONSTRAINT [' + @var21 + '];');
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
                    AND [UserId] NOT IN (SELECT [Id] FROM [AspNetUsers])
                
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
                    AND [AspnetuserId] NOT IN (SELECT [Id] FROM [AspNetUsers])
                
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
                    AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers])
                
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
                    AND [UserID] NOT IN (SELECT [Id] FROM [AspNetUsers])
                
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
                    AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo])
                
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
                    AND [FoundBy] NOT IN (SELECT [Id] FROM [AspNetUsers])
                
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
                    AND [MissingPetsID] NOT IN (SELECT [Id] FROM [MissingPet])
                
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
                    AND [PetId] NOT IN (SELECT [Id] FROM [PetInfo])
                
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

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624140427_AddPetTypeMasterTable'
)
BEGIN
    DECLARE @var22 sysname;
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PetInfo]') AND [c].[name] = N'PetType');
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [PetInfo] DROP CONSTRAINT [' + @var22 + '];');
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

