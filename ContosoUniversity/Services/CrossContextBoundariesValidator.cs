namespace ContosoUniversity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Departments.Models;
    using Data.Students.Models;

    /// <summary>
    ///     TODO: these checks should be implemented in domain/service layers later
    /// </summary>
    public static class CrossContextBoundariesValidator
    {
        /// <summary>
        ///     Ensure all assigned courses reference existing department records
        /// </summary>
        public static void EnsureCoursesReferenceTheExistingDepartments(
            IEnumerable<Domain.Course> courses,
            Dictionary<Guid, string> departmentNames)
        {
            var notFoundDepartments = courses
                .Select(x => x.DepartmentId)
                .Distinct()
                .Where(x => !departmentNames.ContainsKey(x))
                .ToArray();

            if (notFoundDepartments.Any())
            {
                var notFoundList = string.Join(", ", notFoundDepartments);
                throw new Exception($"Unbound contexts inconsistency. Departments not found: {notFoundList}.");
            }
        }

        /// <summary>
        ///     Ensure all assigned courses reference existing course records
        /// </summary>
        public static void EnsureInstructorsReferenceTheExistingCourses(
            IEnumerable<Instructor> instructors,
            IEnumerable<Domain.Course> courses)
        {
            var referencedCourseIds = instructors.SelectMany(x => x.CourseAssignments.Select(ca => ca.CourseExternalId)).ToHashSet();
            var existingCourseIds = courses.Select(x => x.EntityId).ToHashSet();
            var notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

            if (notFoundCourses.Any())
            {
                var notFoundList = string.Join(", ", notFoundCourses);
                throw new Exception($"Unbound contexts inconsistency. Course not found: {notFoundList}.");
            }
        }

        /// <summary>
        ///     Ensure all enrollments reference existing course records
        /// </summary>
        public static void EnsureEnrollmentsReferenceTheExistingCourses(
            IEnumerable<Enrollment> enrollments,
            IEnumerable<Domain.Course> courses)
        {
            var referencedCourseIds = enrollments.Select(x => x.CourseExternalId).ToHashSet();
            var existingCourseIds = courses.Select(x => x.EntityId).ToHashSet();
            var notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

            if (notFoundCourses.Any())
            {
                var notFoundList = string.Join(", ", notFoundCourses);
                throw new Exception($"Unbound contexts inconsistency. Course not found: {notFoundList}.");
            }
        }
    }
}