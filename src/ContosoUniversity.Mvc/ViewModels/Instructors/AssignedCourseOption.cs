namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

public class AssignedCourseOption
{
    public Guid CourseExternalId { get; set; }
    public int CourseCode { get; set; }
    public string Title { get; set; }
    public bool Assigned { get; set; }
}