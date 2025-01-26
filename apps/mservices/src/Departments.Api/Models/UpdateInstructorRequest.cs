namespace Departments.Api.Models;

public record UpdateInstructorRequest(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
