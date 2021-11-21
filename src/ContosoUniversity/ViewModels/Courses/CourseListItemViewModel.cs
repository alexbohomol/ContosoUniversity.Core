namespace ContosoUniversity.ViewModels.Courses;

using System;
using System.ComponentModel.DataAnnotations;

public class CourseListItemViewModel
{
    [Display(Name = "Course Code")] public int CourseCode { get; init; }

    public string Title { get; init; }
    public int Credits { get; init; }
    public string Department { get; init; }
    public Guid Id { get; init; }
}