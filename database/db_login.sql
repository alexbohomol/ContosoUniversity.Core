USE master
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