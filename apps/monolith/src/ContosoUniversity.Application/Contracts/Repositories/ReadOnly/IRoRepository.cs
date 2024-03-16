namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IRoRepository<TProjection> where TProjection : IIdentifiable<Guid>
{
    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<TProjection> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<TProjection[]> GetAll(CancellationToken cancellationToken = default);
}
