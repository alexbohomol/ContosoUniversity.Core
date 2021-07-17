namespace ContosoUniversity.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Departments.Models;

    using Domain.Course;
    using Domain.Student;

    using Instructors;

    public static class DomainExtensions
    {
        public static string ToDisplayString(this Grade grade)
        {
            return grade switch
            {
                Grade.Undefined => "No grade",
                var regular => regular.ToString()
            };
        }
        
        public static AssignedCourseOption[] ToAssignedCourseOptions(
            this IEnumerable<Course> courses,
            Instructor instructor = null)
        {
            var instructorCourses = instructor?
                .CourseAssignments
                .Select(c => c.CourseExternalId) ?? Array.Empty<Guid>();

            return courses.Select(course => new AssignedCourseOption
            {
                CourseCode = course.Code,
                CourseExternalId = course.EntityId,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.EntityId)
            }).ToArray();
        }

    }
}