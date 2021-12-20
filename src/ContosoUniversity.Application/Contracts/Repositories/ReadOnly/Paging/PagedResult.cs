namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);