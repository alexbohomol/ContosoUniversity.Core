namespace ContosoUniversity.Domain.Student;

using System;

public record Enrollment(
    Guid StudentId,
    Guid CourseId,
    Grade? Grade);
