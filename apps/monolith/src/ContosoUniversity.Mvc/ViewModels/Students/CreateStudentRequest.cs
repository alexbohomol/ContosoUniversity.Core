namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;

public class CreateStudentRequest
{
    public DateTime EnrollmentDate { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}
