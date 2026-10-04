#pragma warning disable CA1001
namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Writes;

using Domain.Course;

using Factories;

[MemoryDiagnoser]
public class RepositorySaveVsContextSave
{
    private ReadWriteRepository _repository;
    private ReadWriteContext _context;
    private int _iteration;

    [GlobalSetup]
    public async Task Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();
        const int batchCount = 10;
        const int batchSize = 10_000;
        await _context.PopulateAsync(batchCount, batchSize,
            CoursesFactory.CreateCourse,
            i => Console.WriteLine($"Batch #{i}/{batchCount} inserted. {i * batchSize} records. {DateTime.Now:T}"));
        _iteration = 0;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.CleanupAsync<Course>(x => x.Code == 1234);
        await _context.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public async Task SaveWithRepository()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _repository.Save(course);
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task SaveWithContext()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _context.Set<Course>().AddAsync(course);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
    }
}
