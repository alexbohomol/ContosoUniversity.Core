﻿namespace ContosoUniversity.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Data.Contexts;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using ViewModels;

    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            var groups = new List<EnrollmentDateGroup>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                await using var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT EnrollmentDate, COUNT(*) AS StudentCount
                      FROM [std].Student
                      GROUP BY EnrollmentDate";
                var reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new EnrollmentDateGroup
                            {EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1)};
                        groups.Add(row);
                    }
                }

                reader.Dispose();
            }
            finally
            {
                conn.Close();
            }

            return View(groups);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}