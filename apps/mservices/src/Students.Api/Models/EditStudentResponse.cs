namespace Students.Api.Models;

public record EditStudentResponse(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName,
    Guid ExternalId);
