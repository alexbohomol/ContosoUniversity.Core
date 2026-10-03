SELECT COUNT(*) AS "Total Courses"
FROM [ContosoUniversity].[crs].[Course] WITH(NOLOCK)

SELECT Credits, COUNT(*) AS Count
FROM [ContosoUniversity].[crs].[Course] WITH(NOLOCK)
GROUP BY Credits
ORDER BY Credits

SELECT DepartmentId, COUNT(*) AS Count
FROM [ContosoUniversity].[crs].[Course] WITH(NOLOCK)
GROUP BY DepartmentId
ORDER BY DepartmentId
