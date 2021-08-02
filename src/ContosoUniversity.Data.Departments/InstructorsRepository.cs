namespace ContosoUniversity.Data.Departments
{
    using System.Linq;

    using Domain.Contracts;
    using Domain.Instructor;

    public class InstructorsRepository : EfRepository<Instructor, Models.Instructor>, IInstructorsRepository
    {
        public InstructorsRepository(DepartmentsContext dbContext) 
            : base(
                dbContext, 
                new []
                {
                    nameof(Models.Instructor.OfficeAssignment),
                    nameof(Models.Instructor.CourseAssignments)
                })
        {
        }

        protected override Instructor ToDomainEntity(Models.Instructor dataModel) => new(
            dataModel.FirstMidName,
            dataModel.LastName,
            dataModel.HireDate,
            dataModel.CourseAssignments.Select(x => new CourseAssignment(x.CourseExternalId)).ToArray(),
            new OfficeAssignment(dataModel.OfficeAssignment.Location),
            dataModel.ExternalId);

        protected override void MapDomainEntityOntoDataEntity(Instructor source, Models.Instructor target)
        {
            target.FirstMidName = source.FirstName;
            target.LastName = source.LastName;
            target.HireDate = source.HireDate;
            target.ExternalId = source.EntityId;
            
            /*
             * TODO: specify later
             */
            // target.CourseAssignments = source.Courses;
            // target.OfficeAssignment = source.Office;
        }
    }
}