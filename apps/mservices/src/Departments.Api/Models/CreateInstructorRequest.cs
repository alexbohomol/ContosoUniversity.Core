namespace Departments.Api.Models;

public record CreateInstructorRequest(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
