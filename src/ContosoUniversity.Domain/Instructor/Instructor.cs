namespace ContosoUniversity.Domain.Instructor;

using System;
using System.Collections.Generic;
using System.Linq;

public class Instructor : IIdentifiable<Guid>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;

    private Instructor(
        string firstName,
        string lastName,
        DateTime hireDate,
        OfficeAssignment office)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
        Office = office;
        ExternalId = Guid.NewGuid();
    }

    private Instructor()
    {
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime HireDate { get; private set; }

    public IList<Guid> Courses => Assignments
        .Select(x => x.CourseId)
        .ToList();

    public IList<CourseAssignment> Assignments { get; private set; }

    public OfficeAssignment Office { get; set; }

    public string FullName => $"{LastName}, {FirstName}";

    public Guid ExternalId { get; }

    public static Instructor Create(
        string firstName,
        string lastName,
        DateTime hireDate,
        OfficeAssignment office)
    {
        return new Instructor(firstName, lastName, hireDate, office);
    }

    public void UpdatePersonalInfo(string firstName, string lastName, DateTime hireDate)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
    }

    public void AssignCourses(Guid[] courseIds)
    {
        Assignments = courseIds
            .Select(x => new CourseAssignment(ExternalId, x))
            .ToList();
    }
}