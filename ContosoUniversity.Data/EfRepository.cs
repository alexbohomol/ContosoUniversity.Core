namespace ContosoUniversity.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Domain;
    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public abstract class EfRepository<TDomainEntity, TDataEntity> : IRepository<TDomainEntity> 
        where TDomainEntity : class, IAggregateRoot
        where TDataEntity : class, IExternalIdentifier, new()
    {
        private const string ErrMsgDbUpdateException = "Unable to save changes. Try again, and if the problem persists, see your system administrator.";

        protected readonly DbContext DbContext;

        protected EfRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task<TDomainEntity> GetById(Guid entityId)
        {
            var dataEntity = await DbContext
                .Set<TDataEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == entityId);

            return dataEntity == null
                ? null
                : ToDomainEntity(dataEntity);
        }

        public async Task<TDomainEntity[]> GetAll()
        {
            var dataEntities = await DbContext
                .Set<TDataEntity>()
                .AsNoTracking()
                .ToArrayAsync();
            
            return dataEntities.Select(ToDomainEntity).ToArray();
        }

        public async Task Save(TDomainEntity entity)
        {
            var dataEntity = await DbContext
                .Set<TDataEntity>()
                .FirstOrDefaultAsync(x => x.ExternalId == entity.EntityId);
            
            if (dataEntity == null)
            {
                dataEntity = new TDataEntity();

                MapDomainEntityOntoDataEntity(dataEntity, entity);

                await DbContext.AddAsync(dataEntity);
            }
            else
            {
                MapDomainEntityOntoDataEntity(dataEntity, entity);
            }

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                //Log the error (uncomment ex variable name and write a log.)
                throw new PersistenceException(
                    ErrMsgDbUpdateException,
                    exception);
            }
        }

        public async Task Remove(Guid entityId)
        {
            var dataEntity = await DbContext
                .Set<TDataEntity>()
                .FirstOrDefaultAsync(x => x.ExternalId == entityId);
            if (dataEntity == null)
                throw new EntityNotFoundException(nameof(TDataEntity), entityId);

            DbContext.Set<TDataEntity>().Remove(dataEntity);

            await DbContext.SaveChangesAsync();
        }

        protected abstract TDomainEntity ToDomainEntity(
            TDataEntity dataModel);

        protected abstract void MapDomainEntityOntoDataEntity(
            TDataEntity dataEntity, 
            TDomainEntity domainEntity);
    }
}