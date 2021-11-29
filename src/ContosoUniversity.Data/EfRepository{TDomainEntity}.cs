namespace ContosoUniversity.Data;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain;
using Domain.Contracts;

using Microsoft.EntityFrameworkCore;

public abstract class EfRepository<TDomainEntity>
    : IRepository<TDomainEntity>
    where TDomainEntity : class, IIdentifiable<Guid>
{
    protected readonly DbContext DbContext;
    protected readonly IQueryable<TDomainEntity> DbQuery;
    protected readonly DbSet<TDomainEntity> DbSet;

    public EfRepository(DbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TDomainEntity>();
        DbQuery = DbSet;
    }
    
    public async Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TDomainEntity> GetById(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExternalId == entityId, cancellationToken);
    }

    public async Task<TDomainEntity[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await DbQuery
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task Save(TDomainEntity entity, CancellationToken cancellationToken = default)
    {
        TDomainEntity existing = await DbContext.FindAsync<TDomainEntity>(entity.ExternalId);
        if (existing is null)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }
        else
        {
            DbSet.Update(entity);
        }

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(Guid entityId, CancellationToken cancellationToken = default)
    {
        TDomainEntity existing = await DbContext.FindAsync<TDomainEntity>(entityId);
        if (existing is not null)
        {
            DbSet.Remove(existing);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}