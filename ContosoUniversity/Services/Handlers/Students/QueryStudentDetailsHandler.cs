namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Students;

    using ViewModels;
    using ViewModels.Students;

    public class QueryStudentDetailsHandler : IRequestHandler<QueryStudentDetails, StudentDetailsViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly ICoursesRepository _coursesRepository;

        public QueryStudentDetailsHandler(
            IStudentsRepository studentsRepository,
            ICoursesRepository coursesRepository)
        {
            _studentsRepository = studentsRepository;
            _coursesRepository = coursesRepository;
        }

        public async Task<StudentDetailsViewModel> Handle(QueryStudentDetails request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            var coursesIds = student.Enrollments.Select(x => x.CourseId).ToArray();

            var courseTitles = (await _coursesRepository.GetByIds(coursesIds))
                .ToDictionary(x => x.EntityId, x => x.Title);

            return new StudentDetailsViewModel
            {
                LastName = student.LastName,
                FirstMidName = student.FirstName,
                EnrollmentDate = student.EnrollmentDate,
                ExternalId = student.EntityId,
                Enrollments = student.Enrollments.Select(x => new EnrollmentViewModel
                {
                    CourseTitle = courseTitles[x.CourseId],
                    Grade = x.Grade.ToDisplayString()
                }).ToArray()
            };
        }
    }
}