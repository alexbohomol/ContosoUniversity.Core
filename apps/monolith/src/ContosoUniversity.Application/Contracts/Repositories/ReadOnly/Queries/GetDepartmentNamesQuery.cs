namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Queries;

using System;
using System.Collections.Generic;

using MediatR;

public record GetDepartmentNamesQuery : IRequest<Dictionary<Guid, string>>;
