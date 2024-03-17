namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

public record CreateInstructorRequest
{
    public string LastName { get; init; }
    public string FirstName { get; init; }
    public DateTime HireDate { get; init; } = DateTime.Today;
    public Guid[] SelectedCourses { get; init; }
    public string Location { get; init; }
}
