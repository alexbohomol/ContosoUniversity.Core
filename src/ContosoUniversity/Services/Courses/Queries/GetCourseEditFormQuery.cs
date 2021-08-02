namespace ContosoUniversity.Services.Courses.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using ViewModels.Courses;

    public record GetCourseEditFormQuery(Guid Id) : IRequest<CourseEditForm>;
    
    public class GetCourseEditFormQueryHandler : IRequestHandler<GetCourseEditFormQuery, CourseEditForm>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IDepartmentsRepository _departmentsRepository;

        public GetCourseEditFormQueryHandler(
            ICoursesRepository coursesRepository,
            IDepartmentsRepository departmentsRepository)
        {
            _coursesRepository = coursesRepository;
            _departmentsRepository = departmentsRepository;
        }

        public async Task<CourseEditForm> Handle(GetCourseEditFormQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            var departments = await _departmentsRepository.GetDepartmentNamesReference();

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(
                new [] { course }, 
                departments.Keys);

            return new CourseEditForm(
                new EditCourseCommand(course),
                course.Code,
                departments);
        }
    }
}