namespace ContosoUniversity.Services.Queries.Instructors
{
    using System;

    using MediatR;

    using ViewModels.Instructors;

    public record InstructorEditFormQuery(Guid Id) : IRequest<EditInstructorForm>;
}