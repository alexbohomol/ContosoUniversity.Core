USE master;
GO

IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ContosoUniversity')
BEGIN
    ALTER DATABASE ContosoUniversity SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE ContosoUniversity
END;
GO
