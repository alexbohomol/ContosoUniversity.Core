namespace ContosoUniversity.Data
{
    using System;
    using System.Threading.Tasks;

    using Domain;
    using Domain.Contracts;

    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
    {
        public Task<TEntity> GetById(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Save(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid entityId)
        {
            throw new NotImplementedException();
        }
    }
}