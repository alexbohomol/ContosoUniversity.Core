SELECT COUNT(*) AS "Total Courses"
FROM [ContosoUniversity].[crs].[Course]

SELECT Credits, COUNT(*) AS Count
FROM [ContosoUniversity].[crs].[Course]
GROUP BY Credits
ORDER BY Credits

SELECT DepartmentId, COUNT(*) AS Count
FROM [ContosoUniversity].[crs].[Course]
GROUP BY DepartmentId
ORDER BY DepartmentId
