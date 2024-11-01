namespace ContosoUniversity.SharedKernel.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);
