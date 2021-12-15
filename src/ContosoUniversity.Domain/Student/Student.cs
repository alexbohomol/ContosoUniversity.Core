namespace ContosoUniversity.Domain.Student;

using System;

public class Student : IIdentifiable<Guid>
{
    private Student(
        string lastName,
        string firstName,
        DateTime enrollmentDate)
    {
        LastName = lastName;
        FirstName = firstName;
        EnrollmentDate = enrollmentDate;
        Enrollments = EnrollmentsCollection.Empty;
        ExternalId = Guid.NewGuid();
    }

    private Student()
    {
    }

    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    public EnrollmentsCollection Enrollments { get; }

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

        Enrollments.AddEnrollments(enrollments);
    }

    public void WithdrawCourses(Guid[] courseIds)
    {
        Enrollments.RemoveEnrollments(courseIds);
    }
}