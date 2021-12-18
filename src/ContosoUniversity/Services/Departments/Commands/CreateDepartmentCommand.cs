namespace ContosoUniversity.Services.Departments.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories;

using Domain.Department;

using MediatR;

public class CreateDepartmentCommand : IRequest
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public Guid? AdministratorId { get; set; }
}

public class CreateDepartmentCommandHandler : AsyncRequestHandler<CreateDepartmentCommand>
{
    private readonly IDepartmentsRwRepository _departmentsRepository;

    public CreateDepartmentCommandHandler(IDepartmentsRwRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;
    }

    protected override async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = Department.Create(
            request.Name,
            request.Budget,
            request.StartDate,
            request.AdministratorId);

        await _departmentsRepository.Save(department, cancellationToken);
    }
}