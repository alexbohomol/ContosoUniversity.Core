#pragma warning disable CA1001
#pragma warning disable CA5394
namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Writes;

using Domain.Course;

using Factories;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class RemoveByIdVsRemoveSetOfCourses
{
    private ReadWriteRepository _repository;
    private ReadWriteContext _context;
    private Queue<Guid> _coursesExternalIds;

    [GlobalSetup]
    public async Task Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();

        const int batchCount = 10;
        const int batchSize = 10_000;

        var courses = await _context.PopulateAsync(
            batchCount,
            batchSize,
            CoursesFactory.CreateCourse,
            i => Console.WriteLine($"Batch #{i}/{batchCount} inserted. {i * batchSize} records. {DateTime.Now:T}"));

        _coursesExternalIds = new Queue<Guid>(courses.Select(x => x.ExternalId));

        Console.WriteLine($"Setup: inserted {_coursesExternalIds.Count} courses.");
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        var count = await _context.CleanupAsync<Course>(x => x.Code == 1234);
        Console.WriteLine($"Cleanup: deleted {count} courses.");

        await _context.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public async Task RemoveSingleCourse()
    {
        var id = _coursesExternalIds.Dequeue();
        await _repository.Remove(id);
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task RemoveSetOfCourses()
    {
        Guid[] ids =
        [
            _coursesExternalIds.Dequeue(),
            _coursesExternalIds.Dequeue(),
            _coursesExternalIds.Dequeue(),
            _coursesExternalIds.Dequeue(),
            _coursesExternalIds.Dequeue()
        ];

        await _repository.Remove(ids);
        _context.ChangeTracker.Clear();
    }

    // [Benchmark]
    public async Task RemoveWithContext()
    {
        var id = _coursesExternalIds.Dequeue();
        var course = await _context.Set<Course>().FindAsync(id);
        _context.Set<Course>().Remove(course!);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
    }
}
