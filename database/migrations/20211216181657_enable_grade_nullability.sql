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

