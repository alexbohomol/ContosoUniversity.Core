namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Department;

    public interface IDepartmentsRepository : IRepository<Department>
    {
        Task<Dictionary<Guid, string>> GetDepartmentNamesReference();
        Task<bool> Exists(Guid departmentId);
    }
}