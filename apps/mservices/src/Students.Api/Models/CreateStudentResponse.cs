namespace Students.Api.Models;

public record CreateStudentResponse(
    Guid ExternalId,
    DateTime EnrollmentDate,
    string LastName,
    string FirstName);
