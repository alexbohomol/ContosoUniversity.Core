namespace Courses.Api.Models;

internal record UpdateCourseRequest(
    string Title,
    int Credits,
    Guid DepartmentId);
