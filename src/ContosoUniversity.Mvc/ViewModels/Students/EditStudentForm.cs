namespace ContosoUniversity.Mvc.ViewModels.Students;

using System;

using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Students.Commands;

public class EditStudentForm : EditStudentCommand
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
