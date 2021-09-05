namespace ContosoUniversity.Services.Departments.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;

    using FluentValidation;

    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        private readonly IInstructorsRepository _repository;

        public CreateDepartmentCommandValidator(IInstructorsRepository repository)
        {
            _repository = repository;
            
            RuleFor(x => x).SetInheritanceValidator(x =>
            {
                x.Add(v => new CreateDepartmentFormValidator());
            });
            
            RuleFor(x => x.AdministratorId)
                .MustAsync(BeAnExistingInstructor)
                .WithMessage("Please select an existing instructor.");
        }

        private async Task<bool> BeAnExistingInstructor(Guid? administratorId, CancellationToken token) => 
            administratorId switch
            {
                null => true,
                _ => await _repository.Exists(administratorId.Value, token)
            };
    }
}