namespace Courses.Api.Models;

public record UpdateCourseRequest(
    string Title,
    int Credits,
    Guid DepartmentId);
