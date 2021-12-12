namespace ContosoUniversity.Data.Departments;

using Domain.Contracts;
using Domain.Instructor;

public class InstructorsRwRepository : EfRwRepository<Instructor>, IInstructorsRwRepository
{
    public InstructorsRwRepository(DepartmentsContext dbContext)
        : base(
            dbContext,
            new[]
            {
                nameof(Instructor.Assignments),
                nameof(Instructor.Office)
            })
    {
    }
}