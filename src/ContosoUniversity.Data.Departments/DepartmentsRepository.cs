namespace ContosoUniversity.Data.Departments
{
    using Domain;
    using Domain.Contracts;

    public class DepartmentsRepository : EfRepository<Department, Models.Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(DepartmentsContext dbContext) 
            : base(
                dbContext,
                defaultIncludes: new [] { nameof(Models.Department.Administrator) }) 
        { }

        protected override Department ToDomainEntity(Models.Department dataModel) => new(
            dataModel.Name,
            dataModel.Budget,
            dataModel.StartDate,
            dataModel.Administrator?.ExternalId ?? default,
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