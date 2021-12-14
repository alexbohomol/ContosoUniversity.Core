namespace ContosoUniversity.Data;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain;
using Domain.Contracts;

using Microsoft.EntityFrameworkCore;

public abstract class EfRoRepository<TReadModel> : IRoRepository<TReadModel>
    where TReadModel : class, IIdentifiable<Guid>
{
    protected readonly DbContext DbContext;
    protected readonly IQueryable<TReadModel> DbQuery;
    protected readonly DbSet<TReadModel> DbSet;

    protected EfRoRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TReadModel>();
        DbQuery = DbSet;
    }

    public async Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .AnyAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TReadModel> GetById(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TReadModel[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}