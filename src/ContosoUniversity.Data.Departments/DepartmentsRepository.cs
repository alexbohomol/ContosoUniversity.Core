namespace ContosoUniversity.Data.Departments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
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

        public override async Task<Department> GetById(Guid entityId, CancellationToken cancellationToken = default)
        {
            var department = await DbSet
                .FromSqlInterpolated($"SELECT * FROM [dpt].Department WHERE ExternalId = {entityId}")
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            
            return department == null
                ? null
                : ToDomainEntity(department);
        }

        public Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken = default) => DbSet
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.Name,
                cancellationToken);

        public Task<bool> Exists(Guid departmentId, CancellationToken cancellationToken = default) => DbSet
            .AnyAsync(x => x.ExternalId == departmentId, cancellationToken);
        
        public async Task<Department[]> GetByAdministrator(Guid instructorId, CancellationToken cancellationToken = default)
        {
            var departments = await DbSet
                .AsNoTracking()
                .Where(x => x.Administrator.ExternalId == instructorId)
                .ToArrayAsync(cancellationToken);

            return departments.Select(ToDomainEntity).ToArray();
        }

        protected override Department ToDomainEntity(Models.Department dataModel) => new(
            dataModel.Name,
            dataModel.Budget,
            dataModel.StartDate,
            dataModel.Administrator?.ExternalId,
            dataModel.ExternalId);

        protected override void MapDomainEntityOntoDataEntity(Department domainEntity, Models.Department dataEntity)
        {
            dataEntity.Name = domainEntity.Name;
            dataEntity.Budget = domainEntity.Budget;
            dataEntity.StartDate = domainEntity.StartDate;
            // dataEntity.InstructorId = domainEntity.AdministratorId;
            dataEntity.ExternalId = domainEntity.EntityId;

            /*
             * TODO: git rid of Instructor entity reference
             * It creates a mess with relations here
             */
            if (domainEntity.AdministratorId.HasValue)
            {
                dataEntity.Administrator = ((DepartmentsContext)DbContext)
                                           .Instructors
                                           .FirstOrDefault(x => x.ExternalId == domainEntity.AdministratorId);
            }
            else
            {
                dataEntity.Administrator = null;
            }
        }
    }
}