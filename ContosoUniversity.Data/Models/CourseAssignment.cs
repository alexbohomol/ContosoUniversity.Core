namespace ContosoUniversity.Data.Models
{
    using System;

    public class CourseAssignment
    {
        public int InstructorId { get; set; }
        public Guid CourseExternalId { get; set; }
    }
}