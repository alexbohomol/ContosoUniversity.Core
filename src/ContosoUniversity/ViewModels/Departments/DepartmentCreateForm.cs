namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MediatR;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class DepartmentCreateForm : IRequest
    {
        public SelectList SelectList { get; }

        public DepartmentCreateForm()
        {
            
        }

        public DepartmentCreateForm(DepartmentCreateForm command, IDictionary<int, string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            InstructorId = command.InstructorId;
            SelectList = instructorNames.ToSelectList();
        }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Administrator")]
        public int? InstructorId { get; set; }

        public SelectList InstructorsDropDown { get; set; }
    }
}