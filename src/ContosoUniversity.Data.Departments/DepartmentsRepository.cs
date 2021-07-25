namespace ContosoUniversity.Data.Departments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Department;

    using Microsoft.EntityFrameworkCore;

    public class DepartmentsRepository : EfRepository<Department, Models.Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(DepartmentsContext dbContext) 
            : base(
                dbContext,
                defaultIncludes: new [] { nameof(Models.Department.Administrator) }) 
        { }

        public override async Task<Department> GetById(Guid entityId)
        {
            var department = await DbSet
                .FromSqlInterpolated($"SELECT * FROM [dpt].Department WHERE ExternalId = {entityId}")
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            
            return department == null
                ? null
                : ToDomainEntity(department);
        }

        public Task<Dictionary<Guid, string>> GetDepartmentNamesReference() => DbSet
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToDictionaryAsync(
               x => x.ExternalId,
               x => x.Name);

        protected override Department ToDomainEntity(Models.Department dataModel) => new(
            dataModel.Name,
            dataModel.Budget,
            dataModel.StartDate,
            new Administrator(dataModel.Administrator?.FirstMidName, dataModel.Administrator?.LastName),
            dataModel.ExternalId);

        protected override void MapDomainEntityOntoDataEntity(Department domainEntity, Models.Department dataEntity)
        {
            dataEntity.Name = domainEntity.Name;
            dataEntity.Budget = domainEntity.Budget;
            dataEntity.StartDate = domainEntity.StartDate;
            // dataEntity.InstructorId = domainEntity.AdministratorId;
            dataEntity.ExternalId = domainEntity.EntityId;
        }
    }
}