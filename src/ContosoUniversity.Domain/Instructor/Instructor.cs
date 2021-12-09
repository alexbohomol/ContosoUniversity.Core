namespace ContosoUniversity.Domain.Instructor;

using System;
using System.Collections.Generic;

public class Instructor : IIdentifiable<Guid>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;

    private Instructor(
        string firstName,
        string lastName,
        DateTime hireDate,
        IList<Guid> courses,
        OfficeAssignment office)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
        Courses = courses;
        Office = office;
        ExternalId = Guid.NewGuid();
    }

    private Instructor()
    {
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime HireDate { get; private set; }

    public IList<Guid> Courses { get; set; }

    public IList<CourseAssignment> Assignments { get; set; }

    public OfficeAssignment Office { get; set; }

    public string FullName => $"{LastName}, {FirstName}";

    public Guid ExternalId { get; }

    public static Instructor Create(
        string firstName,
        string lastName,
        DateTime hireDate,
        IList<Guid> courses,
        OfficeAssignment office)
    {
        return new Instructor(firstName, lastName, hireDate, courses, office);
    }

    public void UpdatePersonalInfo(string firstName, string lastName, DateTime hireDate)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
    }
}