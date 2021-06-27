namespace ContosoUniversity.Domain.Student
{
    using System;

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
}