IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

IF SCHEMA_ID(N'dpt') IS NULL EXEC(N'CREATE SCHEMA [dpt];');
GO

CREATE TABLE [dpt].[Department] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Budget] money NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [InstructorId] uniqueidentifier NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dpt].[Instructor] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [HireDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Instructor] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dpt].[CourseAssignment] (
    [InstructorId] uniqueidentifier NOT NULL,
    [CourseId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([InstructorId]),
    CONSTRAINT [FK_CourseAssignment_Instructor_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [dpt].[Instructor] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [dpt].[OfficeAssignment] (
    [InstructorId] uniqueidentifier NOT NULL,
    [Title] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_OfficeAssignment] PRIMARY KEY ([InstructorId]),
    CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorId] FOREIGN KEY ([InstructorId]) REFERENCES [dpt].[Instructor] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211210234830_init', N'6.0.2');
GO

