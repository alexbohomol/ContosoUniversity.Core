namespace ContosoUniversity.Services.Departments.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;

    using FluentValidation;

    public class EditDepartmentCommandValidator : AbstractValidator<EditDepartmentCommand>
    {
        private readonly IInstructorsRepository _repository;

        public EditDepartmentCommandValidator(IInstructorsRepository repository)
        {
            _repository = repository;
            
            RuleFor(x => x).SetInheritanceValidator(x =>
            {
                x.Add(v => new EditDepartmentFormValidator());
            });
            
            When(x => x.AdministratorId.HasValue, () =>
            {
                RuleFor(x => x.AdministratorId)
                    .MustAsync((guid, token) => BeAnExistingInstructor(guid.Value, token))
                    .WithMessage("Please select an existing instructor.");
            });
        }

        private async Task<bool> BeAnExistingInstructor(Guid administratorId, CancellationToken token) => 
            await _repository.Exists(administratorId, token);
    }
}