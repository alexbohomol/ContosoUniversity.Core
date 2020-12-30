namespace ContosoUniversity.ViewModels
{
    using System;

    public class AssignedCourseData
    {
        public Guid CourseUid { get; set; }
        public int CourseCode { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}