namespace ContosoUniversity.Mvc.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Student;

using global::Courses.Core.Projections;

using Instructors;

public static class DomainExtensions
{
    public static string ToDisplayString(this Grade? grade)
    {
        return grade.HasValue ? grade.ToString() : "No grade";
    }

    public static AssignedCourseOption[] ToAssignedCourseOptions(
        this IEnumerable<Course> courses,
        IEnumerable<Guid> instructorCourses = null)
    {
        return courses.Select(course => new AssignedCourseOption
        {
            CourseCode = course.Code,
            CourseExternalId = course.ExternalId,
            Title = course.Title,
            Assigned = instructorCourses?.Contains(course.ExternalId) ?? false
        }).ToArray();
    }
}
