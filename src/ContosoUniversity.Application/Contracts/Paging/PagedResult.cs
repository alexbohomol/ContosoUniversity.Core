namespace ContosoUniversity.Application.Contracts.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);