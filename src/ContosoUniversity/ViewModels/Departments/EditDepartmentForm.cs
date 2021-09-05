namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Departments.Commands;

    public class EditDepartmentForm : EditDepartmentCommand
    {
        public EditDepartmentForm(EditDepartmentCommand command, Dictionary<Guid,string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            AdministratorId = command.AdministratorId;
            ExternalId = command.ExternalId;
            RowVersion = command.RowVersion;
            InstructorsDropDown = instructorNames.ToSelectList(command.AdministratorId.GetValueOrDefault());
        }

        public EditDepartmentForm()
        {
            
        }

        public SelectList InstructorsDropDown { get; set; }
    }
}