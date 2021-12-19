namespace ContosoUniversity.Services.Departments.Queries;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;

using MediatR;

public record GetDepartmentsIndexQuery : IRequest<GetDepartmentsIndexQueryResult>;

public record GetDepartmentsIndexQueryResult(
    Department[] Departments,
    Dictionary<Guid, string> InstructorsReference);

public class GetDepartmentsIndexQueryHandler :
    IRequestHandler<GetDepartmentsIndexQuery, GetDepartmentsIndexQueryResult>
{
    private readonly IDepartmentsRoRepository _departmentsRepository;
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetDepartmentsIndexQueryHandler(
        IInstructorsRoRepository instructorsRepository,
        IDepartmentsRoRepository departmentsRepository)
    {
        _instructorsRepository = instructorsRepository;
        _departmentsRepository = departmentsRepository;
    }

    public async Task<GetDepartmentsIndexQueryResult> Handle(
        GetDepartmentsIndexQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

#warning This is potential place to introduce view in scope of dpt-schema. Instructors names can be included in read model,
        Department[] departments = await _departmentsRepository.GetAll(cancellationToken);

        Dictionary<Guid, string> instructorsReference = await _instructorsRepository
            .GetInstructorNamesReference(cancellationToken);

        return new GetDepartmentsIndexQueryResult(departments, instructorsReference);
    }
}