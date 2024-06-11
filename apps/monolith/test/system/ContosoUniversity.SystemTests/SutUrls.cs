namespace ContosoUniversity.SystemTests;

using Microsoft.Extensions.Configuration;

public class SutUrls(IConfiguration configuration)
{
    public readonly string BaseAddress = $"{configuration["PageBaseUrl:Http"]}";
    public string CoursesCreatePage => $"{BaseAddress}/Courses/Create";
    public string CoursesDeletePage => $"{BaseAddress}/Courses/Delete";
    public string CoursesEditPage => $"{BaseAddress}/Courses/Edit";
    public string CoursesListPage => $"{BaseAddress}/Courses";
    public string StudentsCreatePage => $"{BaseAddress}/Students/Create";
    public string StudentsDeletePage => $"{BaseAddress}/Students/Delete";
    public string StudentsEditPage => $"{BaseAddress}/Students/Edit";
    public string StudentsListPage => $"{BaseAddress}/Students";
}
