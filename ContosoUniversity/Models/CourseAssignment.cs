namespace ContosoUniversity.Models
{
    using System;

    public class CourseAssignment
    {
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }
        public Guid CourseUid { get; set; }
    }
}