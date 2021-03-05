namespace ContosoUniversity.Domain
{
    using System;

    public class Course : IAggregateRoot
    {
        public Course(
            CourseCode code,
            string title,
            Credits credits,
            Guid departmentId) 
            : this(
                code, 
                title, 
                credits, 
                departmentId, 
                Guid.NewGuid())
        { }

        public Course(
            CourseCode code,
            string title,
            Credits credits,
            Guid departmentId,
            Guid entityId)
        {
            Code = code;
            Title = title;
            Credits = credits;
            DepartmentId = departmentId;
            EntityId = entityId;
        }

        public CourseCode Code { get; }
        public string Title { get; }
        public Credits Credits { get; }
        public Guid DepartmentId { get; }
        public Guid EntityId { get; }
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