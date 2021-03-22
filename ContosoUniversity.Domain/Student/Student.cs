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
}