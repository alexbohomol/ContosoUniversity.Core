using IInstructorsRoRepository = Departments.Core.IInstructorsRoRepository;
using Instructor = Departments.Core.Projections.Instructor;

namespace ContosoUniversity.Application.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetInstructorEditFormQuery(Guid Id) : IRequest<GetInstructorEditFormQueryResult>;

public record GetInstructorEditFormQueryResult(Instructor Instructor, Course[] Courses);

internal class GetInstructorEditFormQueryHandler(
    IInstructorsRoRepository instructorsRepository,
    ICoursesApiClient coursesApiClient)
    : IRequestHandler<GetInstructorEditFormQuery, GetInstructorEditFormQueryResult>
{
    public async Task<GetInstructorEditFormQueryResult> Handle(
        GetInstructorEditFormQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Instructor instructor = await instructorsRepository.GetById(request.Id, cancellationToken);
        if (instructor is null)
        {
            throw new EntityNotFoundException(nameof(instructor), request.Id);
        }

        Course[] courses = await coursesApiClient.GetAll(cancellationToken);

        return new GetInstructorEditFormQueryResult(instructor, courses);
    }
}
