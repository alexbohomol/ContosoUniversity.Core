namespace ContosoUniversity.Domain.Student;

using System;

public class Student : IIdentifiable<Guid>
{
    public Student(
        string lastName,
        string firstName,
        DateTime enrollmentDate,
        EnrollmentsCollection enrollments,
        Guid externalId)
    {
        LastName = lastName;
        FirstName = firstName;
        EnrollmentDate = enrollmentDate;
        Enrollments = enrollments;
        ExternalId = externalId;
    }

    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public EnrollmentsCollection Enrollments { get; }

    public Guid ExternalId { get; }

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

        Enrollments.AddEnrollments(enrollments);
    }

    public void WithdrawCourses(Guid[] courseIds)
    {
        Enrollments.RemoveEnrollments(courseIds);
    }
}