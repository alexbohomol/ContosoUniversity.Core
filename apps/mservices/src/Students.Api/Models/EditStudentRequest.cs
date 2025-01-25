namespace Students.Api.Models;

public record EditStudentRequest(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName,
    Guid ExternalId);
