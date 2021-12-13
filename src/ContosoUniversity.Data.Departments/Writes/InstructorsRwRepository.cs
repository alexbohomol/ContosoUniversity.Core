namespace ContosoUniversity.Data.Departments.Writes;

using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class InstructorsRwRepository : EfRwRepository<Instructor>, IInstructorsRwRepository
{
    public InstructorsRwRepository(ReadWriteContext dbContext)
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