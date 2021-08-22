namespace ContosoUniversity.Data
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain;
    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public abstract class EfRepository<TDomainEntity, TDataEntity> 
        : IRepository<TDomainEntity> 
            where TDomainEntity : class, IIdentifiable<Guid>
            where TDataEntity : class, IExternalIdentifier, new()
    {
        private const string ErrMsgDbUpdateException = "Unable to save changes. Try again, and if the problem persists, see your system administrator.";

        protected readonly DbContext DbContext;
        protected readonly DbSet<TDataEntity> DbSet;
        protected readonly IQueryable<TDataEntity> DbQuery;

        protected EfRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = DbContext.Set<TDataEntity>();
            DbQuery = DbSet;
        }

        /// <summary>
        /// https://gist.github.com/oneillci/3205384
        /// </summary>
        protected EfRepository(DbContext dbContext, string[] defaultIncludes) : this(dbContext)
        {
            DbQuery = defaultIncludes.Aggregate(
                DbQuery, 
                (dbQuery, relationProperty) => dbQuery.Include(relationProperty));
        }
        
        public virtual async Task<TDomainEntity> GetById(Guid entityId, CancellationToken cancellationToken = default)
        {
            var dataEntity = await DbQuery
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == entityId, cancellationToken);

            return dataEntity == null
                ? null
                : ToDomainEntity(dataEntity);
        }

        public virtual async Task<TDomainEntity[]> GetAll(CancellationToken cancellationToken = default)
        {
            var dataEntities = await DbQuery
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
            
            return dataEntities.Select(ToDomainEntity).ToArray();
        }

        public async Task Save(TDomainEntity entity, CancellationToken cancellationToken = default)
        {
            var dataEntity = await DbQuery
                .FirstOrDefaultAsync(x => x.ExternalId == entity.EntityId);
            
            if (dataEntity == null)
            {
                dataEntity = new TDataEntity();

                MapDomainEntityOntoDataEntity(entity, dataEntity);

                DbSet.Add(dataEntity);
            }
            else
            {
                MapDomainEntityOntoDataEntity(entity, dataEntity);
            }

            try
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException exception)
            {
                //Log the error (uncomment ex variable name and write a log.)
                throw new PersistenceException(
                    ErrMsgDbUpdateException,
                    exception);
            }
        }

        public async Task Remove(Guid entityId, CancellationToken cancellationToken = default)
        {
            var dataEntity = await DbQuery
                .FirstOrDefaultAsync(x => x.ExternalId == entityId, cancellationToken);
            if (dataEntity == null)
                throw new EntityNotFoundException(nameof(TDataEntity), entityId);

            DbSet.Remove(dataEntity);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        protected abstract TDomainEntity ToDomainEntity(
            TDataEntity dataModel);

        protected abstract void MapDomainEntityOntoDataEntity(
            TDomainEntity domainEntity, 
            TDataEntity dataEntity);
    }
}