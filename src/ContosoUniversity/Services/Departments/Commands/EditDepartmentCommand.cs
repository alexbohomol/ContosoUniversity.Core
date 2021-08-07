namespace ContosoUniversity.Services.Departments.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

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
        private readonly IDepartmentsRepository _departmentsRepository;
        private readonly IInstructorsRepository _instructorsRepository;

        public EditDepartmentCommandHandler(
            IDepartmentsRepository departmentsRepository,
            IInstructorsRepository instructorsRepository)
        {
            _departmentsRepository = departmentsRepository;
            _instructorsRepository = instructorsRepository;
        }
        
        protected override async Task Handle(EditDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentsRepository.GetById(request.ExternalId);

            department.UpdateGeneralInfo(request.Name, request.Budget, request.StartDate);
            
            if (request.AdministratorId.HasValue)
            {
                if (!await _instructorsRepository.Exists(request.AdministratorId.Value))
                    throw new EntityNotFoundException("instructor", request.AdministratorId.Value);
                
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