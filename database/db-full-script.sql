
-- :r db-create.sql

USE [master];
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[databases] WHERE name = N'ContosoUniversity')
BEGIN
    CREATE DATABASE [ContosoUniversity]
END;
GO

USE [ContosoUniversity]
GO

CREATE SCHEMA [crs]
GO
CREATE SCHEMA [dpt]
GO
CREATE SCHEMA [std]
GO

-- :r db-login.sql

USE [master]
GO

/*
 * Courses Schema
 */

CREATE LOGIN [courses_ro] WITH PASSWORD = 'coursesRO-P@$$w0rd'
GO

CREATE LOGIN [courses_rw] WITH PASSWORD = 'coursesRW-P@$$w0rd'
GO

/*
 * Departments Schema
 */

CREATE LOGIN [departments_ro] WITH PASSWORD = 'departmentsRO-P@$$w0rd'
GO

CREATE LOGIN [departments_rw] WITH PASSWORD = 'departmentsRW-P@$$w0rd'
GO

/*
 * Students Schema
 */

CREATE LOGIN [students_ro] WITH PASSWORD = 'studentsRO-P@$$w0rd'
GO

CREATE LOGIN [students_rw] WITH PASSWORD = 'studentsRW-P@$$w0rd'
GO

/*
DROP LOGIN courses_ro
DROP LOGIN courses_rw
DROP LOGIN departments_ro
DROP LOGIN departments_rw
DROP LOGIN students_ro
DROP LOGIN students_rw
*/

-- :r db-user.sql

USE [ContosoUniversity]
GO

/*
 * Courses Schema
 */

CREATE USER [courses_ro] FOR LOGIN [courses_ro] WITH DEFAULT_SCHEMA=[crs]
GO
ALTER ROLE [db_datareader] ADD MEMBER [courses_ro]
GO

CREATE USER [courses_rw] FOR LOGIN [courses_rw] WITH DEFAULT_SCHEMA=[crs]
GO
ALTER ROLE [db_datareader] ADD MEMBER [courses_rw]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [courses_rw]
GO
-- we need it temporarily to enable migrations with 'dotnet ef'
ALTER ROLE [db_ddladmin] ADD MEMBER [courses_rw]
GO

/*
 * Departments Schema
 */

CREATE USER [departments_ro] FOR LOGIN [departments_ro] WITH DEFAULT_SCHEMA=[dpt]
GO
ALTER ROLE [db_datareader] ADD MEMBER [departments_ro]
GO

CREATE USER [departments_rw] FOR LOGIN [departments_rw] WITH DEFAULT_SCHEMA=[dpt]
GO
ALTER ROLE [db_datareader] ADD MEMBER [departments_rw]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [departments_rw]
GO
-- we need it temporarily to enable migrations with 'dotnet ef'
ALTER ROLE [db_ddladmin] ADD MEMBER [departments_rw]
GO

/*
 * Students Schema
 */

CREATE USER [students_ro] FOR LOGIN [students_ro] WITH DEFAULT_SCHEMA=[std]
GO
ALTER ROLE [db_datareader] ADD MEMBER [students_ro]
GO

CREATE USER [students_rw] FOR LOGIN [students_rw] WITH DEFAULT_SCHEMA=[std]
GO
ALTER ROLE [db_datareader] ADD MEMBER [students_rw]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [students_rw]
GO
-- we need it temporarily to enable migrations with 'dotnet ef'
ALTER ROLE [db_ddladmin] ADD MEMBER [students_rw]
GO

/*
DROP USER courses_ro
DROP USER courses_rw
DROP USER departments_ro
DROP USER departments_rw
DROP USER students_ro
DROP USER students_rw
*/

-- :r db-migrate.sql

-- :r ./migrations/20211204153755_init.sql

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

-- :r ./migrations/20211210234830_init.sql

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

-- :r ./migrations/20211211114553_remove_pk_courseassignments.sql

ALTER TABLE [dpt].[CourseAssignment] DROP CONSTRAINT [PK_CourseAssignment];
GO

ALTER TABLE [dpt].[CourseAssignment] ADD CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([InstructorId], [CourseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211211114553_remove_pk_courseassignments', N'6.0.2');
GO

-- :r ./migrations/20211215202621_init.sql

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

-- :r ./migrations/20211216181657_enable_grade_nullability.sql

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[std].[Enrollment]') AND [c].[name] = N'Grade');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [std].[Enrollment] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [std].[Enrollment] ALTER COLUMN [Grade] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211216181657_enable_grade_nullability', N'6.0.2');
GO

-- :r ./migrations/20211224221137_add_view_departmentprojection.sql

CREATE VIEW [dpt].[DepartmentProjection] AS
SELECT 
  [d].[Id], 
  [d].[InstructorId] AS [AdministratorId], 
  [d].[Budget], 
  [d].[Name], 
  [d].[StartDate], 
  [i].[LastName] AS [AdministratorLastName], 
  [i].[FirstName] AS [AdministratorFirstName]
FROM [dpt].[Department] AS [d]
LEFT OUTER JOIN [dpt].[Instructor] AS [i] 
  ON [d].[InstructorId] = [i].[Id]
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211224221137_add_view_departmentprojection', N'6.0.2');
GO

-- :r db-seed.sql

USE ContosoUniversity
GO

INSERT INTO [crs].[Course] ([Id], [CourseCode], [Title], [Credits], [DepartmentId])
VALUES
    ('51f60b7d-fb0c-40eb-a74b-b2d90157afa0', 2042, 'Literature',     4, '377c186a-6782-4367-9246-e5fe4195a97c'),
    ('7f4a2bf3-8623-4d4b-a555-7e1c18da1d31', 2021, 'Composition',    3, '377c186a-6782-4367-9246-e5fe4195a97c'),
    ('42153736-0a08-49ef-84a1-7718189945ca', 3141, 'Trigonometry',   4, '72c0804d-b208-4e67-82ba-cf54dc93dcc8'),
    ('f3e9966c-467b-4b99-90ca-a29bae85ca94', 1045, 'Calculus',       4, '72c0804d-b208-4e67-82ba-cf54dc93dcc8'),
    ('8ebb5543-371a-4c5b-a72b-09bc9f615e36', 4041, 'Macroeconomics', 3, '31a130fe-b396-4bb8-88d3-26fa8778b4c6'),
    ('d53ffc3d-aa4e-41cf-8f0e-435c73889dcf', 4022, 'Microeconomics', 3, '31a130fe-b396-4bb8-88d3-26fa8778b4c6'),
    ('1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4', 1050, 'Chemistry',      3, 'dab7e678-e3e7-4471-8282-96fe52e5c16f')
GO

INSERT INTO [dpt].[Instructor] ([HireDate], [LastName], [FirstName], [Id])
VALUES
    ('2004-02-12', 'Zheng',       'Roger',   'deb90ed8-3622-40f6-8b30-8006cd8cb44a'),
    ('2001-01-15', 'Kapoor',      'Candace', '46020553-4c58-4cca-a41f-afa84c29212f'),
    ('1998-07-01', 'Harui',       'Roger',   '43f2feeb-5aae-4a57-b38c-b2e1c1a7b294'),
    ('2002-07-06', 'Fakhouri',    'Fadi',    '0ae7bff6-fdad-45a2-9d24-2cb060fdb3eb'),
    ('1995-03-11', 'Abercrombie', 'Kim',     'd07964d7-466d-48e8-b517-2cad010b12ff')
GO

INSERT INTO [dpt].[Department] ([Name], [Budget], [StartDate], [InstructorId], [Id])
VALUES
    ('Economics',   100000.0000,	N'2007-09-01', '46020553-4c58-4cca-a41f-afa84c29212f', '31a130fe-b396-4bb8-88d3-26fa8778b4c6'),
    ('Engineering', 350000.0000,	N'2007-09-01', '43f2feeb-5aae-4a57-b38c-b2e1c1a7b294', 'dab7e678-e3e7-4471-8282-96fe52e5c16f'),
    ('Mathematics', 100000.0000,	N'2007-09-01', '0ae7bff6-fdad-45a2-9d24-2cb060fdb3eb', '72c0804d-b208-4e67-82ba-cf54dc93dcc8'),
    ('English',     350000.0000,	N'2007-09-01', 'd07964d7-466d-48e8-b517-2cad010b12ff', '377c186a-6782-4367-9246-e5fe4195a97c')
GO

INSERT INTO [dpt].[OfficeAssignment] ([InstructorId], [Title])
VALUES
    ('46020553-4c58-4cca-a41f-afa84c29212f', 'Thompson 304'),
    ('43f2feeb-5aae-4a57-b38c-b2e1c1a7b294', 'Gowan 27'),
    ('0ae7bff6-fdad-45a2-9d24-2cb060fdb3eb', 'Smith 17')
GO

INSERT INTO [dpt].[CourseAssignment] ([InstructorId], [CourseId])
VALUES
    ('deb90ed8-3622-40f6-8b30-8006cd8cb44a', '8ebb5543-371a-4c5b-a72b-09bc9f615e36'),
    ('deb90ed8-3622-40f6-8b30-8006cd8cb44a', 'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    ('46020553-4c58-4cca-a41f-afa84c29212f', '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    ('43f2feeb-5aae-4a57-b38c-b2e1c1a7b294', '42153736-0a08-49ef-84a1-7718189945ca'),
    ('43f2feeb-5aae-4a57-b38c-b2e1c1a7b294', '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    ('0ae7bff6-fdad-45a2-9d24-2cb060fdb3eb', 'f3e9966c-467b-4b99-90ca-a29bae85ca94'),
    ('d07964d7-466d-48e8-b517-2cad010b12ff', '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    ('d07964d7-466d-48e8-b517-2cad010b12ff', '51f60b7d-fb0c-40eb-a74b-b2d90157afa0')
GO

INSERT INTO [std].[Student] ([EnrollmentDate], [LastName], [FirstName], [Id])
VALUES
    ('2010-09-01', 'Alexander', 'Carson',   '66179938-bb14-432c-9942-be122c075d82'),
    ('2012-09-01', 'Alonso',    'Meredith', 'df921e0d-99de-4108-8adf-c38ee8691dcd'),
    ('2013-09-01', 'Anand',     'Arturo',   '19470df6-3c71-4781-b33b-20e8826fe142'),
    ('2012-09-01', 'Barzdukas', 'Gytis',    'f2d07cb0-13c1-44b9-9a47-9e3e847b218c'),
    ('2012-09-01', 'Li',        'Yan',      '735810de-cc33-4182-b0ef-09bb4203485c'),
    ('2011-09-01', 'Justice',   'Peggy',    '0135624d-2325-4a9c-933e-90cc24dbef48'),
    ('2013-09-01', 'Norman',    'Laura',    'b8d60b43-1f29-4fa7-894b-2540cb9b511f'),
    ('2005-09-01', 'Olivetto',  'Nino',     '3872880b-40dc-4d8e-892f-a51fb8765f7d')
GO

INSERT INTO [std].[Enrollment] ([StudentId], [Grade], [CourseId])
VALUES
    ('19470df6-3c71-4781-b33b-20e8826fe142',  1,    'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    ('19470df6-3c71-4781-b33b-20e8826fe142',  NULL, '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    ('df921e0d-99de-4108-8adf-c38ee8691dcd',  1,    '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    ('df921e0d-99de-4108-8adf-c38ee8691dcd',  1,    '42153736-0a08-49ef-84a1-7718189945ca'),
    ('df921e0d-99de-4108-8adf-c38ee8691dcd',  1,    'f3e9966c-467b-4b99-90ca-a29bae85ca94'),
    ('66179938-bb14-432c-9942-be122c075d82',  1,    '8ebb5543-371a-4c5b-a72b-09bc9f615e36'),
    ('66179938-bb14-432c-9942-be122c075d82',  2,    'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    ('66179938-bb14-432c-9942-be122c075d82',  0,    '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    ('f2d07cb0-13c1-44b9-9a47-9e3e847b218c',  1,    '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    ('735810de-cc33-4182-b0ef-09bb4203485c',  1,    '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    ('0135624d-2325-4a9c-933e-90cc24dbef48',  1,    '51f60b7d-fb0c-40eb-a74b-b2d90157afa0')
GO
