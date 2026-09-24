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
    public string InstructorsCreatePage => $"{BaseAddress}/Instructors/Create";
    public string InstructorsDeletePage => $"{BaseAddress}/Instructors/Delete";
    public string InstructorsEditPage => $"{BaseAddress}/Instructors/Edit";
    public string InstructorsListPage => $"{BaseAddress}/Instructors";
    public string StudentsCreatePage => $"{BaseAddress}/Students/Create";
    public string StudentsDeletePage => $"{BaseAddress}/Students/Delete";
    public string StudentsEditPage => $"{BaseAddress}/Students/Edit";
    public string StudentsListPage => $"{BaseAddress}/Students";
}
