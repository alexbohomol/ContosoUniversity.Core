namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Students;

    using ViewModels.Students;

    public class QueryStudentDeletePageHandler : IRequestHandler<QueryStudentDeletePage, StudentDeleteViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;

        public QueryStudentDeletePageHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task<StudentDeleteViewModel> Handle(QueryStudentDeletePage request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            return new StudentDeleteViewModel
            {
                LastName = student.LastName,
                FirstMidName = student.FirstName,
                EnrollmentDate = student.EnrollmentDate,
                ExternalId = student.EntityId
            };
        }
    }
}