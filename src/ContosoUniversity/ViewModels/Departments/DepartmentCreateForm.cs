namespace ContosoUniversity.ViewModels.Departments
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Commands.Departments;

    public class CreateDepartmentForm : CreateDepartmentCommand
    {
        public CreateDepartmentForm()
        {
            
        }

        public CreateDepartmentForm(CreateDepartmentCommand command, IDictionary<int, string> instructorNames)
        {
            Name = command.Name;
            Budget = command.Budget;
            StartDate = command.StartDate;
            InstructorId = command.InstructorId;
            InstructorsDropDown = instructorNames.ToSelectList();
        }

        public SelectList InstructorsDropDown { get; set; }
    }
}