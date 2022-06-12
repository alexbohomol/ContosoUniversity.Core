IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

IF SCHEMA_ID(N'std') IS NULL EXEC(N'CREATE SCHEMA [std];');
GO

CREATE TABLE [std].[Student] (
    [Id] uniqueidentifier NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [EnrollmentDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [std].[Enrollment] (
    [StudentId] uniqueidentifier NOT NULL,
    [CourseId] uniqueidentifier NOT NULL,
    [Grade] int NOT NULL,
    CONSTRAINT [PK_Enrollment] PRIMARY KEY ([StudentId], [CourseId]),
    CONSTRAINT [FK_Enrollment_Student_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [std].[Student] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211215202621_init', N'6.0.2');
GO

