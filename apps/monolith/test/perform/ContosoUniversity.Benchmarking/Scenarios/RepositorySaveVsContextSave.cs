#pragma warning disable CA1001
namespace ContosoUniversity.Benchmarking.Scenarios;

using System.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Writes;

using Domain.Course;

using Factories;

using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
public class RepositorySaveVsContextSave
{
    private ReadWriteRepository _repository;
    private ReadWriteContext _context;
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();
        _iteration = 0;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _context.Set<Course>().Where(x => x.Code == 1234).ExecuteDelete();
        _context.Dispose();
    }

    [Benchmark(Baseline = true)]
    public async Task SaveRepository()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _repository.Save(course);
    }

    [Benchmark]
    public async Task SaveContext()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
    }
}
