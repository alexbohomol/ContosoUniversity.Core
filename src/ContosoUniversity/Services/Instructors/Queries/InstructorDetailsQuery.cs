namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;

    using MediatR;

    using ViewModels.Instructors;

    public record InstructorDetailsQuery(Guid Id) : IRequest<InstructorDetailsViewModel>;
}