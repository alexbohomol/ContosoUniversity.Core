namespace ContosoUniversity.Domain.Student
{
    using System;
    using System.Collections.Generic;

    public class Student : IAggregateRoot
    {
        public Student(
            string lastName,
            string firstName,
            DateTime enrollmentDate,
            Enrollment[] enrollments,
            Guid entityId)
        {
            LastName = lastName;
            FirstName = firstName;
            EnrollmentDate = enrollmentDate;
            Enrollments = enrollments;
            EntityId = entityId;
        }

        public string LastName { get; }
        public string FirstName { get; }
        public DateTime EnrollmentDate { get; }
        public ICollection<Enrollment> Enrollments { get; }
        public Guid EntityId { get; }
    }
    
    public struct Enrollment
    {
        private readonly Guid _courseId;
        
        public Enrollment(Guid courseId, Grade grade) : this()
        {
            CourseId = courseId;
            Grade = grade;
        }

        public Grade Grade { get; }

        public Guid CourseId
        {
            get => _courseId;
            private init
            {
                if (value == default)
                    throw new ArgumentException(
                        "Course Id cannot be of default value.");

                _courseId = value;
            }
        }
    }
    
    public enum Grade
    {
        Undefined,
        A,
        B,
        C,
        D,
        F
    }

    public static class GradeExtensions
    {
        public static string ToDisplayString(this Grade grade)
        {
            return grade switch
            {
                Grade.Undefined => "No grade",
                var regular => regular.ToString()
            };
        }
    }
}