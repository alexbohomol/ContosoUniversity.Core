namespace Students.Core.Paging;

public record PagedResult<TEntity>(TEntity[] Items, PageInfo Info);
