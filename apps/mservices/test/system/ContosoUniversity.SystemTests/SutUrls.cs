namespace ContosoUniversity.SystemTests;

using Microsoft.Extensions.Configuration;

public class SutUrls(IConfiguration configuration)
{
    public string BaseAddress { get; } = configuration["PageBaseUrl:Http"] ?? string.Empty;
    public string CoursesCreatePage => $"{BaseAddress}/Courses/Create";
    public string CoursesDeletePage => $"{BaseAddress}/Courses/Delete";
    public string CoursesEditPage => $"{BaseAddress}/Courses/Edit";
    public string CoursesListPage => $"{BaseAddress}/Courses";
    public string DepartmentsCreatePage => $"{BaseAddress}/Departments/Create";
    public string DepartmentsDeletePage => $"{BaseAddress}/Departments/Delete";
    public string DepartmentsEditPage => $"{BaseAddress}/Departments/Edit";
    public string DepartmentsListPage => $"{BaseAddress}/Departments";
    public string StudentsCreatePage => $"{BaseAddress}/Students/Create";
    public string StudentsDeletePage => $"{BaseAddress}/Students/Delete";
    public string StudentsEditPage => $"{BaseAddress}/Students/Edit";
    public string StudentsListPage => $"{BaseAddress}/Students";
}
