namespace Students.Api.Models;

public record CreateStudentRequest(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName);
