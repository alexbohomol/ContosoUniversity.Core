namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

public interface IRoRepository<TEntity> where TEntity : IIdentifiable<Guid>
{
    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<TEntity> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<TEntity[]> GetAll(CancellationToken cancellationToken = default);
}