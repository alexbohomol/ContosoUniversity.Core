namespace Courses.Api.Models;

public record UpdateCourseResponse(
    Guid ExternalId,
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);
