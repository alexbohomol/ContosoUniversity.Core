namespace ContosoUniversity.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Course : IUniqueEntity
    {
        public int Id { get; set; }
        
        [Display(Name = "Course Code")]
        public int CourseCode { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Guid DepartmentUid { get; set; }
        public Guid UniqueId { get; set; }
    }
}