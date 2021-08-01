namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Departments.Commands;

    public class CreateDepartmentForm : CreateDepartmentCommand
    {
        public CreateDepartmentForm()
        {
            
        }

        public CreateDepartmentForm(CreateDepartmentCommand command, IDictionary<Guid, string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            AdministratorId = command.AdministratorId;
            InstructorsDropDown = instructorNames.ToSelectList();
        }

        public SelectList InstructorsDropDown { get; set; }
    }
}