namespace ContosoUniversity.Application;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IRwRepository<TEntity> where TEntity : IIdentifiable<Guid>
{
    Task<TEntity> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task Save(TEntity entity, CancellationToken cancellationToken = default);
    Task Remove(Guid entityId, CancellationToken cancellationToken = default);
}