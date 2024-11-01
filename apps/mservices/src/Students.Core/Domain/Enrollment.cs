namespace Students.Core.Domain;

using System;

public record Enrollment(
    Guid StudentId,
    Guid CourseId,
    Grade? Grade);
