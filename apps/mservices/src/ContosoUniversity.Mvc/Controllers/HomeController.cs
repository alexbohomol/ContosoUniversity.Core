namespace ContosoUniversity.Mvc.Controllers;

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Students.Core;
using Students.Core.Projections;

using ViewModels;

public class HomeController(IStudentsRoRepository repository) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult> About(CancellationToken cancellationToken)
    {
        EnrollmentDateGroup[] groups = await repository.GetEnrollmentDateGroups(cancellationToken);

        ViewModels.Home.EnrollmentDateGroup[] viewModels = groups.Select(x => new ViewModels.Home.EnrollmentDateGroup
        {
            EnrollmentDate = x.EnrollmentDate,
            StudentCount = x.StudentCount
        }).ToArray();

        return View(viewModels);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
