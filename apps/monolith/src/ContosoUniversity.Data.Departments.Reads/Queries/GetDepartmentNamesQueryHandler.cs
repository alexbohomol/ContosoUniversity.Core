namespace ContosoUniversity.Data.Departments.Reads.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Contracts.Repositories.ReadOnly.Queries;

using MediatR;

using Microsoft.EntityFrameworkCore;

internal class GetDepartmentNamesQueryHandler(
    ReadOnlyContext dbContext) :
    IRequestHandler<GetDepartmentNamesQuery, Dictionary<Guid, string>>
{
    public async Task<Dictionary<Guid, string>> Handle(
        GetDepartmentNamesQuery request,
        CancellationToken cancellationToken) =>
        await dbContext
            .Set<Department>()
            .AsNoTracking()
            .Select(x => new
            {
                x.ExternalId,
                x.Name
            })
            .OrderBy(x => x.Name)
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.Name,
                cancellationToken);
}
