namespace ContosoUniversity.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Departments;

    using Microsoft.EntityFrameworkCore;

    public static class ContextsExtensions
    {
        public static Task<Dictionary<int, string>> GetInstructorsNames(this DepartmentsContext context)
        {
            return context
                   .Instructors
                   .AsNoTracking()
                   .ToDictionaryAsync(
                       x => x.Id,
                       x => x.FullName);
        }
    }
}