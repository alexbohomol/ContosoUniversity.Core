ALTER TABLE [dpt].[CourseAssignment] DROP CONSTRAINT [PK_CourseAssignment];
GO

ALTER TABLE [dpt].[CourseAssignment] ADD CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([InstructorId], [CourseId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211211114553_remove_pk_courseassignments', N'6.0.2');
GO

