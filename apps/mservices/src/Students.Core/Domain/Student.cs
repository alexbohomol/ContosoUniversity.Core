namespace Students.Core.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

using ContosoUniversity.SharedKernel;

public class Student : IIdentifiable<Guid>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;

    private Student(
        string lastName,
        string firstName,
        DateTime enrollmentDate)
    {
        LastName = lastName;
        FirstName = firstName;
        EnrollmentDate = enrollmentDate;
        Enrollments = new List<Enrollment>();
        ExternalId = Guid.NewGuid();
    }

    private Student()
    {
    }

    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public List<Enrollment> Enrollments { get; }

    public Guid ExternalId { get; }

    public static Student Create(
        string lastName,
        string firstName,
        DateTime enrollmentDate)
    {
        return new Student(
            lastName,
            firstName,
            enrollmentDate);
    }

    public void UpdatePersonInfo(string lastName, string firstName)
    {
        LastName = lastName;
        FirstName = firstName;
    }

    public void Enroll(DateTime enrollmentDate)
    {
        EnrollmentDate = enrollmentDate;
    }

    public void EnrollCourses(Enrollment[] enrollments)
    {
        /*
         * TODO: next feature requirements
         * - existing enrollment?
         * - updated grade for the existing enrollment?
         */

        Enrollments.AddRange(enrollments);
    }

    public void WithdrawCourses(Guid[] courseIds)
    {
        Enrollments.RemoveAll(x => courseIds.Contains(x.CourseId));
    }
}
