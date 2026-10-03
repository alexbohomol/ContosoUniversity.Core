#pragma warning disable CA1001
namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
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
    public async Task Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();
        const int batchCount = 10;
        const int batchSize = 10_000;
        await _context.Populate(batchCount, batchSize,
            CoursesFactory.CreateCourse,
            i => Console.WriteLine($"Batch #{i}/{batchCount} inserted. {i * batchSize} records. {DateTime.Now:T}"));
        _context.ChangeTracker.Clear();
        _iteration = 0;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.Set<Course>().Where(x => x.Code == 1234).ExecuteDeleteAsync();
        await _context.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public async Task SaveRepository()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _repository.Save(course);
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task SaveContext()
    {
        _iteration++;
        var course = CoursesFactory.CreateCourse(_iteration);
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
    }
}
