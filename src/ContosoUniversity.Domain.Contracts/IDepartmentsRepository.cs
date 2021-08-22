namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Department;

    public interface IDepartmentsRepository : IRepository<Department>
    {
        Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default);
        Task<bool> Exists(Guid departmentId, CancellationToken cancellationToken = default);
        Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default);
    }
}