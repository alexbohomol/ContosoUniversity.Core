namespace Students.Api.Models;

using ContosoUniversity.SharedKernel.Paging;

internal record SearchModel(
    SearchRequest SearchRequest,
    OrderRequest OrderRequest,
    PageRequest PageRequest);
