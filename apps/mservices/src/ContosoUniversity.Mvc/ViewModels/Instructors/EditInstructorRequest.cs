namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

using global::Departments.Core.Projections;

public record EditInstructorRequest
{
    public EditInstructorRequest(Instructor instructor)
    {
        ExternalId = instructor.ExternalId;
        LastName = instructor.LastName;
        FirstName = instructor.FirstName;
        HireDate = instructor.HireDate;
        Location = instructor.Office;
        SelectedCourses = instructor.Courses;
    }

    public EditInstructorRequest() { }

    public Guid ExternalId { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
    public DateTime HireDate { get; init; }
    public Guid[] SelectedCourses { get; init; }
    public string Location { get; init; }
}
