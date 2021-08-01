namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Commands.Departments;

    public class DepartmentEditForm : EditDepartmentCommand
    {
        public DepartmentEditForm(EditDepartmentCommand command, Dictionary<Guid,string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            AdministratorId = command.AdministratorId;
            ExternalId = command.ExternalId;
            RowVersion = command.RowVersion;
            InstructorsDropDown = instructorNames.ToSelectList(command.AdministratorId.GetValueOrDefault());
        }

        public DepartmentEditForm()
        {
            
        }

        public SelectList InstructorsDropDown { get; set; }
    }
}