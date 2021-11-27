USE ContosoUniversity
GO

SET IDENTITY_INSERT [crs].[Course] ON
GO
INSERT INTO [crs].[Course] ([Id], [CourseCode], [Title], [Credits], [DepartmentExternalId], [ExternalId])
VALUES
    (1, 2042, 'Literature',     4, '377c186a-6782-4367-9246-e5fe4195a97c', '51f60b7d-fb0c-40eb-a74b-b2d90157afa0'),
    (2, 2021, 'Composition',    3, '377c186a-6782-4367-9246-e5fe4195a97c', '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    (3, 3141, 'Trigonometry',   4, '72c0804d-b208-4e67-82ba-cf54dc93dcc8', '42153736-0a08-49ef-84a1-7718189945ca'),
    (4, 1045, 'Calculus',       4, '72c0804d-b208-4e67-82ba-cf54dc93dcc8', 'f3e9966c-467b-4b99-90ca-a29bae85ca94'),
    (5, 4041, 'Macroeconomics', 3, '31a130fe-b396-4bb8-88d3-26fa8778b4c6', '8ebb5543-371a-4c5b-a72b-09bc9f615e36'),
    (6, 4022, 'Microeconomics', 3, '31a130fe-b396-4bb8-88d3-26fa8778b4c6', 'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    (7, 1050, 'Chemistry',      3, 'dab7e678-e3e7-4471-8282-96fe52e5c16f', '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4')
GO
SET IDENTITY_INSERT [crs].[Course] OFF
GO

SET IDENTITY_INSERT [dpt].[Instructor] ON
GO
INSERT INTO [dpt].[Instructor] ([Id], [HireDate], [LastName], [FirstName], [ExternalId])
VALUES
    (1, '2004-02-12 00:00:00.0000000', 'Zheng',       'Roger',   'deb90ed8-3622-40f6-8b30-8006cd8cb44a'),
    (2, '2001-01-15 00:00:00.0000000', 'Kapoor',      'Candace', '46020553-4c58-4cca-a41f-afa84c29212f'),
    (3, '1998-07-01 00:00:00.0000000', 'Harui',       'Roger',   '43f2feeb-5aae-4a57-b38c-b2e1c1a7b294'),
    (4, '2002-07-06 00:00:00.0000000', 'Fakhouri',    'Fadi',    '0ae7bff6-fdad-45a2-9d24-2cb060fdb3eb'),
    (5, '1995-03-11 00:00:00.0000000', 'Abercrombie', 'Kim',     'd07964d7-466d-48e8-b517-2cad010b12ff')
GO
SET IDENTITY_INSERT [dpt].[Instructor] OFF
GO

SET IDENTITY_INSERT [dpt].[Department] ON
GO
INSERT INTO [dpt].[Department] ([Id], [Name], [Budget], [StartDate], [InstructorId], [ExternalId])
VALUES
    -- (1, 'Temp',        0.0000,      CAST(N'2021-02-13' AS DateTime2), NULL, 'eea6f36d-6211-460d-89f9-b760d08f2f9b'),
    (1, 'Economics',   100000.0000,	CAST(N'2007-09-01' AS DateTime2), 2, '31a130fe-b396-4bb8-88d3-26fa8778b4c6'),
    (2, 'Engineering', 350000.0000,	CAST(N'2007-09-01' AS DateTime2), 3, 'dab7e678-e3e7-4471-8282-96fe52e5c16f'),
    (3, 'Mathematics', 100000.0000,	CAST(N'2007-09-01' AS DateTime2), 4, '72c0804d-b208-4e67-82ba-cf54dc93dcc8'),
    (4, 'English',     350000.0000,	CAST(N'2007-09-01' AS DateTime2), 5, '377c186a-6782-4367-9246-e5fe4195a97c')
GO
SET IDENTITY_INSERT [dpt].[Department] OFF
GO

INSERT INTO [dpt].[CourseAssignment] ([InstructorId], [CourseExternalId])
VALUES
    (1, '8ebb5543-371a-4c5b-a72b-09bc9f615e36'),
    (1, 'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    (2, '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    (3, '42153736-0a08-49ef-84a1-7718189945ca'),
    (3, '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    (4, 'f3e9966c-467b-4b99-90ca-a29bae85ca94'),
    (5, '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    (5, '51f60b7d-fb0c-40eb-a74b-b2d90157afa0')
GO

SET IDENTITY_INSERT [std].[Student] ON
GO
INSERT INTO [std].[Student] ([Id], [EnrollmentDate], [LastName], [FirstName], [ExternalId])
VALUES
    (1, '2010-09-01', 'Alexander', 'Carson',   '66179938-bb14-432c-9942-be122c075d82'),
    (2, '2012-09-01', 'Alonso',    'Meredith', 'df921e0d-99de-4108-8adf-c38ee8691dcd'),
    (3, '2013-09-01', 'Anand',     'Arturo',   '19470df6-3c71-4781-b33b-20e8826fe142'),
    (4, '2012-09-01', 'Barzdukas', 'Gytis',    'f2d07cb0-13c1-44b9-9a47-9e3e847b218c'),
    (5, '2012-09-01', 'Li',        'Yan',      '735810de-cc33-4182-b0ef-09bb4203485c'),
    (6, '2011-09-01', 'Justice',   'Peggy',    '0135624d-2325-4a9c-933e-90cc24dbef48'),
    (7, '2013-09-01', 'Norman',    'Laura',    'b8d60b43-1f29-4fa7-894b-2540cb9b511f'),
    (8, '2005-09-01', 'Olivetto',  'Nino',     '3872880b-40dc-4d8e-892f-a51fb8765f7d')
GO
SET IDENTITY_INSERT [std].[Student] OFF
GO

SET IDENTITY_INSERT [std].[Enrollment] ON
GO
INSERT INTO [std].[Enrollment] ([Id], [StudentId], [Grade], [CourseExternalId])
VALUES
    (1,  3,  1,    'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    (2,  3,  NULL, '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    (3,  2,  1,    '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    (4,  2,  1,    '42153736-0a08-49ef-84a1-7718189945ca'),
    (5,  2,  1,    'f3e9966c-467b-4b99-90ca-a29bae85ca94'),
    (6,  1,  1,    '8ebb5543-371a-4c5b-a72b-09bc9f615e36'),
    (7,  1,  2,    'd53ffc3d-aa4e-41cf-8f0e-435c73889dcf'),
    (8,  1,  0,    '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    (9,  4,  1,    '1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4'),
    (10, 5,  1,    '7f4a2bf3-8623-4d4b-a555-7e1c18da1d31'),
    (11, 6,  1,    '51f60b7d-fb0c-40eb-a74b-b2d90157afa0')
GO
SET IDENTITY_INSERT [std].[Enrollment] OFF
GO
