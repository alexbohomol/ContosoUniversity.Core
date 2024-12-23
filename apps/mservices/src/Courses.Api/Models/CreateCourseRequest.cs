namespace Courses.Api.Models;

internal record CreateCourseRequest(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);
