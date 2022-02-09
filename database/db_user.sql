USE ContosoUniversity
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

/*
DROP USER courses_ro
DROP USER courses_rw
DROP USER departments_ro
DROP USER departments_rw
DROP USER students_ro
DROP USER students_rw
*/