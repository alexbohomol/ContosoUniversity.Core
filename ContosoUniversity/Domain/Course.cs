namespace ContosoUniversity.Domain
{
    using System;

    public class Course : IAggregateRoot
    {
        private Course()
        {
            ExternalId = Guid.NewGuid();
        }

        public static Course Create(
            CourseCode code,
            string title,
            Credits credits,
            Guid departmentId) => new()
        {
            Code = code,
            Title = title,
            Credits = credits,
            DepartmentId = departmentId
        };
        
        public CourseCode Code { get; private init; }
        public string Title { get; private init; }
        public Credits Credits { get; private init; }
        public Guid DepartmentId { get; private init; }
        public Guid ExternalId { get; }
    }

    public class Department
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        // public Instructor Administrator { get; set; }
        public Guid ExternalId { get; set; }
    }
}