namespace Departments.Api.Models;

public record EditInstructorResponse(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses);
