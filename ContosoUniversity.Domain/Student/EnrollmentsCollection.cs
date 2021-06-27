namespace ContosoUniversity.Domain.Student
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: is it worth making this immutable?
    /// </summary>
    public class EnrollmentsCollection : IEnumerable<Enrollment>
    {
        private readonly List<Enrollment> _enrollments;

        private EnrollmentsCollection(IEnumerable<Enrollment> enrollments)
        {
            _enrollments = enrollments.ToList();
        }

        public static EnrollmentsCollection Empty => new(Enumerable.Empty<Enrollment>());
        public static EnrollmentsCollection From(IEnumerable<Enrollment> enrollments) =>
            enrollments is not null 
                ? new EnrollmentsCollection(enrollments) 
                : throw new ArgumentNullException(nameof(enrollments));

        public void AddEnrollments(IEnumerable<Enrollment> enrollments)
        {
            _enrollments.AddRange(enrollments);
        }

        public void RemoveEnrollments(Guid[] courseIds)
        {
            var notEnrolledIds = courseIds.Except(CourseIds).ToArray();
            if (notEnrolledIds.Any())
                throw new Exception(
                    $"Request contains ids of not enrolled courses: {notEnrolledIds.ToDisplayString()}.");

            _enrollments.RemoveAll(x => courseIds.Contains(x.CourseId));
        }

        public Enrollment this[Guid courseId] => _enrollments.Single(x => x.CourseId == courseId);

        public IEnumerable<Guid> CourseIds => _enrollments.Select(x => x.CourseId);

        #region Implement IEnumerable<Enrollment>
        
        public IEnumerator<Enrollment> GetEnumerator() => _enrollments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _enrollments.GetEnumerator();
        
        #endregion
    }
}