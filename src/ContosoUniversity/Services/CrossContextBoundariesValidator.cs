namespace ContosoUniversity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Domain;
    using Domain.Course;
    using Domain.Instructor;
    using Domain.Student;

    /// <summary>
    ///     TODO: these checks should be implemented in domain/service layers later
    /// </summary>
    public static class CrossContextBoundariesValidator
    {
        /// <summary>
        ///     Ensure all assigned courses reference existing department records
        /// </summary>
        public static void EnsureCoursesReferenceTheExistingDepartments(
            IEnumerable<Course> courses,
            IEnumerable<Guid> existingDepartmentIds)
        {
            var referencedDepartmentIds = courses.Select(x => x.DepartmentId).ToHashSet();
            var notFoundDepartments = referencedDepartmentIds.Except(existingDepartmentIds).ToArray();

            if (notFoundDepartments.Any())
                throw new Exception(
                    $"Unbound contexts inconsistency. Departments not found: {notFoundDepartments.ToDisplayString()}.");
        }

        /// <summary>
        ///     Ensure all assigned courses reference existing course records
        /// </summary>
        public static void EnsureInstructorsReferenceTheExistingCourses(
            IEnumerable<Instructor> instructors,
            IEnumerable<Course> courses)
        {
            var referencedCourseIds = instructors.SelectMany(x => x.Courses).ToHashSet();
            var existingCourseIds = courses.Select(x => x.EntityId).ToHashSet();
            var notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

            if (notFoundCourses.Any())
                throw new Exception(
                    $"Unbound contexts inconsistency. Course not found: {notFoundCourses.ToDisplayString()}.");
        }

        /// <summary>
        ///     Ensure all enrollments reference existing course records
        /// </summary>
        public static void EnsureEnrollmentsReferenceTheExistingCourses(
            IEnumerable<Enrollment> enrollments,
            IEnumerable<Course> courses)
        {
            var referencedCourseIds = enrollments.Select(x => x.CourseId).ToHashSet();
            var existingCourseIds = courses.Select(x => x.EntityId).ToHashSet();
            var notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

            if (notFoundCourses.Any())
                throw new Exception(
                    $"Unbound contexts inconsistency. Course not found: {notFoundCourses.ToDisplayString()}.");
        }
    }
}