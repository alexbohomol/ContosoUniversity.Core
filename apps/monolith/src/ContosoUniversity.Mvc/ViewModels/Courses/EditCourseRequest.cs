namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class EditCourseRequest
{
    public EditCourseRequest(Course course)
    {
        Id = course.ExternalId;
        Title = course.Title;
        Credits = course.Credits;
        DepartmentId = course.DepartmentId;
    }

    public EditCourseRequest()
    {
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public Guid DepartmentId { get; set; }
}
