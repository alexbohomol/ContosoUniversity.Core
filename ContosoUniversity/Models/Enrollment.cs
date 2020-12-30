namespace ContosoUniversity.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }
        public Guid CourseUid { get; set; }
        public Student Student { get; set; }
    }
}