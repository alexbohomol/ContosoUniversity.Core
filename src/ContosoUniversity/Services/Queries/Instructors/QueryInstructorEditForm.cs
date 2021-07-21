namespace ContosoUniversity.Services.Queries.Instructors
{
    using System;

    using MediatR;

    using ViewModels.Instructors;

    public record QueryInstructorEditForm(Guid Id) : IRequest<InstructorEditForm>;
}