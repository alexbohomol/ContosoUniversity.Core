namespace ContosoUniversity.Data;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;

using Domain;

using Microsoft.EntityFrameworkCore;

public abstract class EfRoRepository<TProjection> : IRoRepository<TProjection>
    where TProjection : class, IIdentifiable<Guid>
{
    protected DbContext DbContext { get; set; }
    protected IQueryable<TProjection> DbQuery { get; set; }
    protected DbSet<TProjection> DbSet { get; set; }

    protected EfRoRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TProjection>();
        DbQuery = DbSet;
    }

    public async Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .AnyAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TProjection> GetById(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TProjection[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}
