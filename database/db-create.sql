USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ContosoUniversity')
BEGIN
    CREATE DATABASE [ContosoUniversity]
END;
GO
