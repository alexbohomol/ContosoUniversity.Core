
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

