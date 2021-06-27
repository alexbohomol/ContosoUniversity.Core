namespace ContosoUniversity.Services.Handlers.Courses
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Courses;

    using Data.Departments;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Courses;

    using ViewModels.Courses;

    public class QueryCourseEditFormHandler : IRequestHandler<QueryCourseEditForm, EditCourseForm>
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly DepartmentsContext _departmentsContext;

        public QueryCourseEditFormHandler(
            ICoursesRepository coursesRepository,
            DepartmentsContext departmentsContext)
        {
            _coursesRepository = coursesRepository;
            _departmentsContext = departmentsContext;
        }

        public async Task<EditCourseForm> Handle(QueryCourseEditForm request, CancellationToken cancellationToken)
        {
            var course = await _coursesRepository.GetById(request.Id);
            if (course == null)
                throw new EntityNotFoundException(nameof(course), request.Id);

            Dictionary<Guid,string> departments = await _departmentsContext.GetDepartmentsNames();

            CrossContextBoundariesValidator.EnsureCoursesReferenceTheExistingDepartments(
                new [] { course }, 
                departments.Keys);

            return new EditCourseForm(
                new EditCourseCommand(course),
                course.Code,
                departments);
        }
    }
}