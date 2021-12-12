namespace ContosoUniversity.Data.Departments;

using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

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

    public async Task<Instructor[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbQuery.ToArrayAsync(cancellationToken);
    }
}