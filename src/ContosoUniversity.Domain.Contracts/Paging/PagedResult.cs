namespace ContosoUniversity.Domain.Contracts.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);