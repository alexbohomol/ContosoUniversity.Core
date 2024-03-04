namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

public class CreateInstructorRequest
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime HireDate { get; set; }
    public Guid[] SelectedCourses { get; set; }
    public string Location { get; set; }
}
