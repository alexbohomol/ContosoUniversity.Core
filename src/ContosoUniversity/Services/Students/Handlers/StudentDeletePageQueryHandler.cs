namespace ContosoUniversity.Services.Students.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries;

    using ViewModels.Students;

    public class StudentDeletePageQueryHandler : IRequestHandler<StudentDeletePageQuery, StudentDeletePageViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentDeletePageQueryHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task<StudentDeletePageViewModel> Handle(StudentDeletePageQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            return new StudentDeletePageViewModel
            {
                LastName = student.LastName,
                FirstMidName = student.FirstName,
                EnrollmentDate = student.EnrollmentDate,
                ExternalId = student.EntityId
            };
        }
    }
}