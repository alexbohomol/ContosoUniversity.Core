namespace ContosoUniversity.Application.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);