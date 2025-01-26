namespace Students.Api.Models;

public record UpdateStudentResponse(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName,
    Guid ExternalId);
