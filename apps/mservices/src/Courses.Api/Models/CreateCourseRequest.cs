namespace Courses.Api.Models;

public record CreateCourseRequest(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);
