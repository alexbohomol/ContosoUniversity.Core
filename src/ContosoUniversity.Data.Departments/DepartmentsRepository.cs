namespace ContosoUniversity.Data.Departments
{
    using Domain;
    using Domain.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class DepartmentsRepository : EfRepository<Department, Models.Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(DbContext dbContext) : base(dbContext) { }

        protected override Department ToDomainEntity(Models.Department dataModel) => new(
            dataModel.Name,
            dataModel.Budget,
            dataModel.StartDate,
            dataModel.Administrator.ExternalId,
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