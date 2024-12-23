namespace Departments.Api.Models;

internal record EditInstructorRequest(
    Guid ExternalId,
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
