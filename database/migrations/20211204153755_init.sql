IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

IF SCHEMA_ID(N'crs') IS NULL EXEC(N'CREATE SCHEMA [crs];');
GO

CREATE TABLE [crs].[Course] (
    [Id] uniqueidentifier NOT NULL,
    [CourseCode] int NOT NULL,
    [Title] nvarchar(50) NOT NULL,
    [Credits] int NOT NULL,
    [DepartmentId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211204153755_init', N'6.0.2');
GO

