namespace ContosoUniversity.Domain.Contracts
{
    using System;
    using System.Threading.Tasks;
    
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        Task<TEntity> GetById(Guid entityId);
        Task<TEntity[]> GetAll();
        Task Save(TEntity entity);
        Task Remove(Guid entityId);
    }
}