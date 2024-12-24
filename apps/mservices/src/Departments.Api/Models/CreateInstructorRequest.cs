namespace Departments.Api.Models;

internal record CreateInstructorRequest(
    string LastName,
    string FirstName,
    DateTime HireDate,
    Guid[] SelectedCourses,
    string Location);
