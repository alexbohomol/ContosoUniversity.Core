namespace ContosoUniversity.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Course : IExternalIdentifier
    {
        public int Id { get; set; }

        [Display(Name = "Course Code")]
        public int CourseCode { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Guid DepartmentExternalId { get; set; }
        public Guid ExternalId { get; set; }
    }
}