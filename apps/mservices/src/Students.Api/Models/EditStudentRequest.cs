namespace Students.Api.Models;

internal record EditStudentRequest(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName,
    Guid ExternalId);
