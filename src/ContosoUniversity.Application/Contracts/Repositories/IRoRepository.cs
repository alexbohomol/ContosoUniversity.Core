namespace ContosoUniversity.Application.Contracts.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IRoRepository<TReadModel> where TReadModel : IIdentifiable<Guid>
{
    Task<bool> Exists(Guid entityId, CancellationToken cancellationToken = default);
    Task<TReadModel> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task<TReadModel[]> GetAll(CancellationToken cancellationToken = default);
}