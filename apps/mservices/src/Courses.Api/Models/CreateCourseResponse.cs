namespace Courses.Api.Models;

public record CreateCourseResponse(
    Guid ExternalId,
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);
