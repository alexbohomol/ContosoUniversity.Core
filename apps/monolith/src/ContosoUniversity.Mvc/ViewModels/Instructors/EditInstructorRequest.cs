namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

public class EditInstructorRequest
{
    public Guid ExternalId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime HireDate { get; set; }
    public Guid[] SelectedCourses { get; set; }
    public string Location { get; set; }
}
