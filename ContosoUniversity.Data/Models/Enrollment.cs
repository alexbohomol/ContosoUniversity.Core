namespace ContosoUniversity.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum Grade
    {
        A,
        B,
        C,
        D,
        F
    }

    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Guid CourseExternalId { get; set; }
        public Student Student { get; set; }
    }
}