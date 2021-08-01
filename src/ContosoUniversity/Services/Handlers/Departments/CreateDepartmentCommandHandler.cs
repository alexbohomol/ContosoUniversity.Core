namespace ContosoUniversity.Services.Handlers.Departments
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Departments;

    using Data.Departments;
    using Data.Departments.Models;

    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class CreateDepartmentCommandHandler : AsyncRequestHandler<CreateDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CreateDepartmentCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            int? instructorId;
            if (request.InstructorId.HasValue)
            {
                var instructor = await _departmentsContext.Instructors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ExternalId == request.InstructorId);

                instructorId = instructor?.Id ?? throw new EntityNotFoundException(nameof(instructor), request.InstructorId.Value);
            }
            else
            {
                instructorId = null;
            }
            
            _departmentsContext.Add(new Department
            {
                Name = request.Name,
                Budget = request.Budget,
                StartDate = request.StartDate,
                InstructorId = instructorId,
                ExternalId = Guid.NewGuid()
            });
            
            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }
    }
}