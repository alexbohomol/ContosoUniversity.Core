namespace Departments.Core.Domain;

using System;

public record CourseAssignment(Guid InstructorId, Guid CourseId);
