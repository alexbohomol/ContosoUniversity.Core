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
