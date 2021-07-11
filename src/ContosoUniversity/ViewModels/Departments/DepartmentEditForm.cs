namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MediatR;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class DepartmentEditForm : IRequest
    {
        public DepartmentEditForm(DepartmentEditForm command, Dictionary<int,string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            InstructorId = command.InstructorId;
            ExternalId = command.ExternalId;
            RowVersion = command.RowVersion;
            InstructorsDropDown = instructorNames.ToSelectList(command.InstructorId.GetValueOrDefault());
        }

        public DepartmentEditForm()
        {
            
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

        public Guid ExternalId { get; set; }

        public byte[] RowVersion { get; set; }

        public SelectList InstructorsDropDown { get; set; }
    }
}