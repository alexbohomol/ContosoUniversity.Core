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

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public ICollection<Enrollment> Enrollments { get; }
        public Guid EntityId { get; }

        public void Update(string lastName, string firstName, DateTime enrollmentDate)
        {
            LastName = lastName;
            FirstName = firstName;
            EnrollmentDate = enrollmentDate;
        }
    }
}