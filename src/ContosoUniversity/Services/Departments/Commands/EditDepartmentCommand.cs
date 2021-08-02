namespace ContosoUniversity.Services.Departments.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class EditDepartmentCommand : IRequest
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Administrator")]
        public Guid? AdministratorId { get; set; }

        public Guid ExternalId { get; set; }
        public byte[] RowVersion { get; set; }
    }
    
    public class EditDepartmentCommandHandler : AsyncRequestHandler<EditDepartmentCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly IDepartmentsRepository _departmentsRepository;

        public EditDepartmentCommandHandler(DepartmentsContext departmentsContext, IDepartmentsRepository departmentsRepository)
        {
            _departmentsContext = departmentsContext;
            _departmentsRepository = departmentsRepository;
        }
        
        protected override async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.ExternalId);

            department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);
            
            if (request.AdministratorId.HasValue)
            {
                var instructor = await _departmentsContext.Instructors
                                                          .AsNoTracking()
                                                          .FirstOrDefaultAsync(x => x.ExternalId == request.AdministratorId);

                if (instructor is null)
                    throw new EntityNotFoundException(nameof(instructor), request.AdministratorId.Value);
                
                department.AssociateAdministrator(request.AdministratorId.Value);
            }
            else
            {
                department.DisassociateAdministrator();
            }

            await _departmentsRepository.Save(department);
        }
    }
}