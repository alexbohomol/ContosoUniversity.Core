namespace Departments.Api.Models;

public record UpdateInstructorResponse(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
