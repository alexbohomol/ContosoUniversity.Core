namespace ContosoUniversity.ViewModels.Departments
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Commands.Departments;

    public class DepartmentEditForm : EditDepartmentCommand
    {
        public DepartmentEditForm(EditDepartmentCommand command, Dictionary<int,string> instructorNames)
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

        public SelectList InstructorsDropDown { get; set; }
    }
}