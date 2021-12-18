namespace ContosoUniversity.Data.Departments.Writes;

using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadWrite;

using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class InstructorsReadWriteRepository : EfRwRepository<Instructor>, IInstructorsRwRepository
{
    public InstructorsReadWriteRepository(ReadWriteContext dbContext) : base(dbContext)
    {
    }

    public async Task<Instructor[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbQuery.ToArrayAsync(cancellationToken);
    }
}