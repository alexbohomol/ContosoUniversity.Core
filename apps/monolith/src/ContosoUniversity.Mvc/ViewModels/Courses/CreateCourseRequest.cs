namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;

public record CreateCourseRequest
{
    public int CourseCode { get; init; }
    public string Title { get; init; }
    public int Credits { get; init; }
    public Guid DepartmentId { get; init; }
}
