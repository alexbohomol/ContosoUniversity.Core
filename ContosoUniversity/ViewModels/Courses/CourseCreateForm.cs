namespace ContosoUniversity.ViewModels.Courses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CourseCreateForm
    {
        [Display(Name = "Course Code")]
        public int CourseCode { get; set; }
        
        public string Title { get; set; }
        
        public int Credits { get; set; }
        
        [Display(Name = "Department")]
        public Guid DepartmentId { get; set; }
        
        public SelectList DepartmentsSelectList { get; set; }
    }
}