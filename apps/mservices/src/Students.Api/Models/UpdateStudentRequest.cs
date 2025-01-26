namespace Students.Api.Models;

public record UpdateStudentRequest(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName);
