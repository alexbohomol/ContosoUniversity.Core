namespace ContosoUniversity.Services.Students.Queries
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using ViewModels;
    using ViewModels.Students;

    public record GetStudentDetailsQuery(Guid Id) : IRequest<StudentDetailsViewModel>;
    
    public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetailsViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly ICoursesRepository _coursesRepository;

        public GetStudentDetailsQueryHandler(
            IStudentsRepository studentsRepository,
            ICoursesRepository coursesRepository)
        {
            _studentsRepository = studentsRepository;
            _coursesRepository = coursesRepository;
        }

        public async Task<StudentDetailsViewModel> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id, cancellationToken);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            var coursesIds = student.Enrollments.CourseIds.ToArray();

            var courseTitles = (await _coursesRepository.GetByIds(coursesIds, cancellationToken))
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