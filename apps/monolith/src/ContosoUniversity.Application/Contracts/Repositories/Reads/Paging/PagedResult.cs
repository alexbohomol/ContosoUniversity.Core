namespace ContosoUniversity.Application.Contracts.Repositories.Reads.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);
