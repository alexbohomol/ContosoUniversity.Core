namespace Departments.Core.Domain;

using System;
using System.Collections.Generic;
using System.Linq;

using ContosoUniversity.SharedKernel;

public class Instructor : IIdentifiable<Guid>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int LastNameMinLength = 1;

    private OfficeAssignment _officeAssignment;

    private Instructor(
        string firstName,
        string lastName,
        DateTime hireDate)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
        ExternalId = Guid.NewGuid();
    }

    private Instructor()
    {
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime HireDate { get; private set; }

    public List<CourseAssignment> CourseAssignments { get; } = new();

    public Guid ExternalId { get; }

    public OfficeAssignment Office => _officeAssignment;

    public void ResetCourseAssignment(Guid courseId)
    {
        CourseAssignment assignment = CourseAssignments.FirstOrDefault(x => x.CourseId == courseId);
        if (assignment is not null)
        {
            CourseAssignments.Remove(assignment); // publish event here: course assignment was reset
        }
    }

    public static Instructor Create(
        string firstName,
        string lastName,
        DateTime hireDate)
    {
        return new Instructor(firstName, lastName, hireDate);
    }

    public void UpdatePersonalInfo(string firstName, string lastName, DateTime hireDate)
    {
        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
    }

    public void ResetCourseAssignments()
    {
        if (CourseAssignments.Any())
        {
            CourseAssignments.Clear();
        }
    }

    public void AssignCourses(Guid[] courseIds)
    {
        ArgumentNullException.ThrowIfNull(courseIds, nameof(courseIds));

        /*
         * Add newly assigned courses
         */
        foreach (Guid courseId in courseIds)
        {
            if (CourseAssignments.All(x => x.CourseId != courseId))
            {
                CourseAssignments.Add(
                    new CourseAssignment(ExternalId, courseId)); // publish event here: assigned to course
            }
        }

        /*
         * Remove courses that were reset
         */
        Guid[] availableAssignments = CourseAssignments.Select(x => x.CourseId).ToArray();
        foreach (Guid courseId in availableAssignments)
        {
            if (!courseIds.Contains(courseId))
            {
                ResetCourseAssignment(courseId); // publish event here: dis-assigned from course
            }
        }
    }

    public void AssignOffice(OfficeAssignment officeAssignment)
    {
        ArgumentNullException.ThrowIfNull(officeAssignment, nameof(officeAssignment));

        _officeAssignment = officeAssignment;
    }

    public void ResetOffice()
    {
        _officeAssignment = null;
    }
}
