namespace ContosoUniversity.ViewModels.Courses;

using System;
using System.ComponentModel.DataAnnotations;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class CourseDetailsViewModel
{
    public CourseDetailsViewModel(Course course, Department department)
    {
        ArgumentNullException.ThrowIfNull(course, nameof(department));
        ArgumentNullException.ThrowIfNull(department, nameof(department));

        CourseCode = course.Code;
        Title = course.Title;
        Credits = course.Credits;
        Department = department.Name;
        Id = course.ExternalId;
    }

    [Display(Name = "Course Code")] public int CourseCode { get; }

    public string Title { get; }
    public int Credits { get; }
    public string Department { get; }
    public Guid Id { get; }
}