namespace ContosoUniversity.Services.Handlers.Courses
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using Queries.Courses;

    using ViewModels;
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
            {
                return null;
            }

            /*
             * TODO: missing context boundary check when department is null
             */

            return new EditCourseForm
            {
                CourseCode = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentId,
                DepartmentsSelectList = await GetSelectList(course.DepartmentId)
            };
        }

        private async Task<SelectList> GetSelectList(Guid selectedDepartment)
        {
            var departments = await _departmentsContext
                .Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToArrayAsync();

            return departments.ToSelectList(selectedDepartment);
        }
    }
}