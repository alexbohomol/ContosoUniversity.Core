namespace ContosoUniversity.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Course;
using Domain.Instructor;

using MediatR;

using ViewModels;
using ViewModels.Instructors;

public record GetInstructorEditFormQuery(Guid Id) : IRequest<EditInstructorForm>;

public class GetInstructorEditFormQueryHandler : IRequestHandler<GetInstructorEditFormQuery, EditInstructorForm>
{
    private readonly ICoursesRepository _coursesRepository;
    private readonly IInstructorsRepository _instructorsRepository;

    public GetInstructorEditFormQueryHandler(
        IInstructorsRepository instructorsRepository,
        ICoursesRepository coursesRepository)
    {
        _instructorsRepository = instructorsRepository;
        _coursesRepository = coursesRepository;
    }

    public async Task<EditInstructorForm> Handle(GetInstructorEditFormQuery request,
        CancellationToken cancellationToken)
    {
        Instructor instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);
        if (instructor is null)
            return null;

        Course[] courses = await _coursesRepository.GetAll(cancellationToken);

        return new EditInstructorForm
        {
            ExternalId = instructor.ExternalId,
            LastName = instructor.LastName,
            FirstName = instructor.FirstName,
            HireDate = instructor.HireDate,
            Location = instructor.Office?.Title,
            AssignedCourses = courses.ToAssignedCourseOptions(instructor.Courses)
        };
    }
}