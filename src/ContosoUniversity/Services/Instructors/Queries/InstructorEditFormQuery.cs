namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;

    using MediatR;

    using ViewModels.Instructors;

    public record InstructorEditFormQuery(Guid Id) : IRequest<EditInstructorForm>;
}