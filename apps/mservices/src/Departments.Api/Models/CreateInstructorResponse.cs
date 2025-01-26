namespace Departments.Api.Models;

public record CreateInstructorResponse(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
