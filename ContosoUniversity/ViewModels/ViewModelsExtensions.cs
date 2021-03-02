namespace ContosoUniversity.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Data.Departments.Models;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ViewModelsExtensions
    {
        public static SelectList ToSelectList(this IEnumerable<Department> departments, Guid selectedDepartment)
        {
            return new(departments,
                nameof(Department.ExternalId),
                nameof(Department.Name),
                selectedDepartment);
        }
    }
}