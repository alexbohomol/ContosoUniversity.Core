namespace ContosoUniversity.Application.Services.Departments.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Department;

using MediatR;

public record CreateDepartmentCommand(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId) : IRequest;

internal class CreateDepartmentCommandHandler(
    IDepartmentsRwRepository departmentsRepository)
    : IRequestHandler<CreateDepartmentCommand>
{
    public async Task Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await departmentsRepository.Save(
            Department.Create(
                request.Name,
                request.Budget,
                request.StartDate,
                request.AdministratorId),
            cancellationToken);
    }
}
