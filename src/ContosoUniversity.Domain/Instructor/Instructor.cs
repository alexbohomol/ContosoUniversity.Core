namespace ContosoUniversity.Domain.Instructor;

using System;
using System.Collections.Generic;
using System.Linq;

public class Instructor : IIdentifiable<Guid>
{
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;

    private readonly List<CourseAssignment> _courseAssignments = new();
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

    public Guid ExternalId { get; }

    [Obsolete("Will be denounced after introducing specific query later")]
    public bool HasCourseAssigned(Guid courseId)
    {
        return _courseAssignments.Select(x => x.CourseId).Contains(courseId);
    }

    public void ResetCourseAssignment(Guid courseId)
    {
        if (HasCourseAssigned(courseId))
        {
            CourseAssignment assignment = _courseAssignments.First(x => x.CourseId == courseId);
            _courseAssignments.Remove(assignment);

            // publish event here: course assignment was reset
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

    [Obsolete("Should be split into 'ResetAssignments' and 'AssignCourses' to control from outside")]
    public void AssignCourses(Guid[] courseIds)
    {
        if (courseIds is null || !courseIds.Any())
        {
            _courseAssignments.Clear();
            return;
        }

#warning Dirty trick to be refined later
        _courseAssignments.Clear();
        _courseAssignments.AddRange(courseIds.Select(x => new CourseAssignment(
            ExternalId,
            x)));

        /*
         * Sudo-code to refine later
         */

        /*
         * Add newly assigned courses
         */
        // foreach (Guid courseId in courseIds)
        // {
        //     if (!HasCourseAssigned(courseId))
        //     {
        //         _courseAssignments.Add(new CourseAssignment(ExternalId, courseId));
        //         
        //         // publish event here: assigned to course
        //     }
        // }

        /*
         * Remove courses that were reset
         */
        // foreach (Guid courseId in _courseAssignments.Select(x => x.CourseId))
        // {
        //     if (!courseIds.Contains(courseId)) ResetCourseAssignment(courseId);
        // }
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