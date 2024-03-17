namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;

public record CreateStudentRequest
{
    public DateTime EnrollmentDate { get; init; } = DateTime.Today;
    public string LastName { get; init; }
    public string FirstName { get; init; }
}
