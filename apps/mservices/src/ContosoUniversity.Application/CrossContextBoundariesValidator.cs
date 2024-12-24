namespace ContosoUniversity.Application;

using System;
using System.Collections.Generic;
using System.Linq;

using ApiClients;

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
        HashSet<Guid> referencedDepartmentIds = courses.Select(x => x.DepartmentId).ToHashSet();
        Guid[] notFoundDepartments = referencedDepartmentIds.Except(existingDepartmentIds).ToArray();

        if (notFoundDepartments.Any())
        {
            throw new Exception(
                $"Unbound contexts inconsistency. Departments not found: {notFoundDepartments.ToDisplayString()}.");
        }
    }

    /// <summary>
    ///     Ensure all assigned courses reference existing course records
    /// </summary>
    public static void EnsureInstructorsReferenceTheExistingCourses(
        Instructor[] instructors,
        IEnumerable<Course> courses)
    {
        HashSet<Guid> referencedCourseIds = instructors.SelectMany(x => x.Courses).ToHashSet();
        HashSet<Guid> existingCourseIds = courses.Select(x => x.ExternalId).ToHashSet();
        Guid[] notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

        if (notFoundCourses.Any())
        {
            throw new Exception(
                $"Unbound contexts inconsistency. Course not found: {notFoundCourses.ToDisplayString()}.");
        }
    }

    /// <summary>
    ///     Ensure all enrollments reference existing course records
    /// </summary>
    public static void EnsureEnrollmentsReferenceTheExistingCourses(
        IEnumerable<Enrollment> enrollments,
        IEnumerable<Course> courses)
    {
        HashSet<Guid> referencedCourseIds = enrollments.Select(x => x.CourseId).ToHashSet();
        HashSet<Guid> existingCourseIds = courses.Select(x => x.ExternalId).ToHashSet();
        Guid[] notFoundCourses = referencedCourseIds.Except(existingCourseIds).ToArray();

        if (notFoundCourses.Any())
        {
            throw new Exception(
                $"Unbound contexts inconsistency. Course not found: {notFoundCourses.ToDisplayString()}.");
        }
    }
}

static file class Extensions
{
    public static string ToDisplayString(this IEnumerable<Guid> guids)
    {
        return string.Join(", ", guids);
    }
}
