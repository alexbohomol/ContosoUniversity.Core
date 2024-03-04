namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;

public class CreateCourseRequest
{
    public int CourseCode { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public Guid DepartmentId { get; set; }
}
