namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

public interface IRoRepository<TReadModel> where TReadModel : IIdentifiable<Guid>
{
    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<TReadModel> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<TReadModel[]> GetAll(CancellationToken cancellationToken = default);
}