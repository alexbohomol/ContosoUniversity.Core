namespace ContosoUniversity.Domain.Student
{
    using System;

    public class Student : IIdentifiable<Guid>
    {
        private readonly EnrollmentsCollection _enrollments;

        public Student(
            string lastName,
            string firstName,
            DateTime enrollmentDate,
            EnrollmentsCollection enrollments,
            Guid entityId)
        {
            LastName = lastName;
            FirstName = firstName;
            EnrollmentDate = enrollmentDate;
            _enrollments = enrollments;
            EntityId = entityId;
        }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public EnrollmentsCollection Enrollments => _enrollments;
        public Guid EntityId { get; }

        public void UpdatePersonInfo(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }

        public void Enroll(DateTime enrollmentDate)
        {
            EnrollmentDate = enrollmentDate;
        }

        public void EnrollCourses(Enrollment[] enrollments)
        {
            /*
             * TODO: next feature requirements
             * - existing enrollment?
             * - updated grade for the existing enrollment?
             */
            
            _enrollments.AddEnrollments(enrollments);
        }

        public void WithdrawCourses(Guid[] courseIds)
        {
            _enrollments.RemoveEnrollments(courseIds);
        }
    }
}