namespace ContosoUniversity.Application.Services.Departments.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Department;

using MediatR;

public class CreateDepartmentCommand : IRequest
{
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public Guid? AdministratorId { get; set; }
}

internal class CreateDepartmentCommandHandler(IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<CreateDepartmentCommand>
{
    public async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = Department.Create(
            request.Name,
            request.Budget,
            request.StartDate,
            request.AdministratorId);

        await departmentsRepository.Save(department, cancellationToken);
    }
}
