namespace ContosoUniversity.Domain.Instructor;

using System;

public record CourseAssignment(Guid InstructorId, Guid CourseId);
