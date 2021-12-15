namespace ContosoUniversity.Domain.Student;

using System;

public record Enrollment(
    Guid StudentId,
    Guid CourseId,
    Grade Grade);

// public record Enrollment
// {
//     private readonly Guid _courseId;
//
//     public Enrollment(Guid courseId, Grade grade)
//     {
//         CourseId = courseId;
//         Grade = grade;
//     }
//
//     public Grade Grade { get; }
//
//     public Guid CourseId
//     {
//         get => _courseId;
//         private init
//         {
//             if (value == default)
//                 throw new ArgumentException(
//                     "Course Id cannot be of default value.");
//
//             _courseId = value;
//         }
//     }
// }