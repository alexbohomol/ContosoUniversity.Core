#pragma warning disable CA5394
namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
using System.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Reads;
using Data.Courses.Writes;

using Domain.Course;

using Factories;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class GetByIdReadOnlyVsReadWrite
{
    private ReadOnlyRepository _roRepository;
    private ReadOnlyContext _roContext;
    private ReadWriteRepository _rwRepository;
    private ReadWriteContext _rwContext;
    private Guid[] _coursesExternalIds;

    [GlobalSetup]
    public async Task Setup()
    {
        (_roRepository, _roContext) = RepositoryFactory.CoursesReadOnly();
        (_rwRepository, _rwContext) = RepositoryFactory.CoursesReadWrite();

        const int batchCount = 10;
        const int batchSize = 10_000;

        var courses = await _rwContext.PopulateAsync(
            batchCount,
            batchSize,
            CoursesFactory.CreateCourse,
            i => Console.WriteLine($"Batch #{i}/{batchCount} inserted. {i * batchSize} records. {DateTime.Now:T}"));

        _coursesExternalIds = [.. courses.Select(x => x.ExternalId)];
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _rwContext.CleanupAsync<Course>(x => x.Code == 1234);
        await _rwContext.DisposeAsync();
        await _roContext.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public async Task GetByIdWithReadOnlyRepo()
    {
        var index = Random.Shared.Next(_coursesExternalIds.Length);
        var id = _coursesExternalIds[index];
        await _roRepository.GetById(id);
        _roContext.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task GetByIdWithReadWriteRepo()
    {
        var index = Random.Shared.Next(_coursesExternalIds.Length);
        var id = _coursesExternalIds[index];
        await _rwRepository.GetById(id);
        _rwContext.ChangeTracker.Clear();
    }
}
