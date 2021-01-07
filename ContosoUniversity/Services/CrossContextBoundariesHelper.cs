namespace ContosoUniversity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    /// <summary>
    /// TODO: these checks should be implemented in domain/service layers later
    /// </summary>
    public static class CrossContextBoundariesHelper
    {
        /// <summary>
        /// Check if any course references the non-existing (deleted) departments
        /// </summary>
        public static void CheckCoursesAgainstDepartments(
            IEnumerable<Course> courses, 
            Dictionary<Guid, string> departmentNames)
        {
            var notFoundDepartments = courses
                .Select(x => x.DepartmentExternalId)
                .Distinct()
                .Where(x => !departmentNames.ContainsKey(x))
                .ToArray();
            
            if (notFoundDepartments.Any())
            {
                var notFoundList = string.Join(", ", notFoundDepartments);
                throw new Exception($"Unbound contexts inconsistency. Departments not found: {notFoundList}.");
            }
        }
    }
}