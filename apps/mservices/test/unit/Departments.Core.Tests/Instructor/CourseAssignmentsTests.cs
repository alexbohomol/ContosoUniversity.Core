namespace Departments.Core.Tests.Instructor;

using System;

using Departments.Core.Domain;

using FluentAssertions;

using NUnit.Framework;

public class CourseAssignmentsTests
{
    [Test]
    public void AssignCoursesToUnassignedInstructor()
    {
        Instructor instructor = CreateUnassignedInstructor();

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        instructor.AssignCourses(new[] { guid1, guid2, guid3 });

        instructor.CourseAssignments.Should().BeEquivalentTo(new[]
        {
            new CourseAssignment(instructor.ExternalId, guid1),
            new CourseAssignment(instructor.ExternalId, guid3),
            new CourseAssignment(instructor.ExternalId, guid2)
        });
    }

    [Test]
    public void ResetCourseAssignmentsClearsAssignedCourses()
    {
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        Instructor instructor = CreateAssignedInstructor(guid1, guid2, guid3);

        instructor.CourseAssignments.Should().BeEquivalentTo(new[]
        {
            new CourseAssignment(instructor.ExternalId, guid1),
            new CourseAssignment(instructor.ExternalId, guid3),
            new CourseAssignment(instructor.ExternalId, guid2)
        });

        instructor.ResetCourseAssignments();

        instructor.CourseAssignments.Should().BeEmpty();
    }

    [Test]
    public void ReassignCoursesToAssignedInstructor()
    {
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        Instructor instructor = CreateAssignedInstructor(guid1, guid2, guid3);

        instructor.CourseAssignments.Should().BeEquivalentTo(new[]
        {
            new CourseAssignment(instructor.ExternalId, guid1),
            new CourseAssignment(instructor.ExternalId, guid3),
            new CourseAssignment(instructor.ExternalId, guid2)
        });

        var guid4 = Guid.NewGuid();

        instructor.AssignCourses(new[] { guid1, guid3, guid4 });

        instructor.CourseAssignments.Should().BeEquivalentTo(new[]
        {
            new CourseAssignment(instructor.ExternalId, guid1),
            new CourseAssignment(instructor.ExternalId, guid3),
            new CourseAssignment(instructor.ExternalId, guid4)
        });
    }

    private Instructor CreateUnassignedInstructor()
    {
        return Instructor.Create(string.Empty, string.Empty, DateTime.Now);
    }

    private Instructor CreateAssignedInstructor(params Guid[] assignments)
    {
        Instructor instructor = CreateUnassignedInstructor();

        instructor.AssignCourses(assignments);

        return instructor;
    }
}
