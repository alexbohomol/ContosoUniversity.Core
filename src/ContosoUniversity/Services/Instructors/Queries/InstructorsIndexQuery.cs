namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;

    using MediatR;

    using ViewModels.Instructors;

    public record InstructorsIndexQuery(Guid? Id, Guid? CourseExternalId) : IRequest<InstructorIndexViewModel>;
}