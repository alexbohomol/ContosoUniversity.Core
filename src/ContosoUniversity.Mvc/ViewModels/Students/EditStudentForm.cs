namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;

using Application.Contracts.Repositories.ReadOnly.Projections;

public class EditStudentForm : EditStudentRequest
{
    public EditStudentForm(Student student)
    {
        ArgumentNullException.ThrowIfNull(student, nameof(student));

        LastName = student.LastName;
        FirstName = student.FirstName;
        EnrollmentDate = student.EnrollmentDate;
        ExternalId = student.ExternalId;
    }
}
