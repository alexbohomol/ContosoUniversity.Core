namespace Students.Api.Models;

internal record CreateStudentRequest(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName);
