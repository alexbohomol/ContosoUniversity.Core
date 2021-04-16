namespace ContosoUniversity.Domain.Student
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Student : IAggregateRoot
    {
        private readonly List<Enrollment> _enrollments;

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
            _enrollments = enrollments.ToList();
            EntityId = entityId;
        }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public List<Enrollment> Enrollments => _enrollments;
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
            
            _enrollments.AddRange(enrollments);
        }

        public void WithdrawCourses(Guid[] courseIds)
        {
            var notEnrolledIds = courseIds.Except(_enrollments.Select(x => x.CourseId)).ToArray();
            if (notEnrolledIds.Any())
            {
                var ids = string.Join(", ", notEnrolledIds);
                throw new Exception($"Request contains ids of not enrolled courses. Provided ids: {ids}.");
            }
            
            _enrollments.RemoveAll(x => courseIds.Contains(x.CourseId));
        }
    }
}