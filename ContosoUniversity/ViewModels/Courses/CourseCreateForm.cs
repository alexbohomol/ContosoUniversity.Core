namespace ContosoUniversity.ViewModels.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CourseCreateForm
    {
        [Display(Name = "Course Code")]
        public int CourseCode { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Guid DepartmentId { get; set; }

        public SelectList DepartmentsSelectList { get; set; }
    }
}