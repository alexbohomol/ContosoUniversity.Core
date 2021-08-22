namespace ContosoUniversity.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels;
    using ViewModels.Home;

    public class HomeController : Controller
    {
        private readonly IStudentsRepository _repository;

        public HomeController(IStudentsRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About(CancellationToken cancellationToken)
        {
            var groups = await _repository.GetEnrollmentDateGroups(cancellationToken);

            var viewModels = groups.Select(x => new EnrollmentDateGroup
            {
                EnrollmentDate = x.EnrollmentDate,
                StudentCount = x.StudentCount
            }).ToArray();
            
            return View(viewModels);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}