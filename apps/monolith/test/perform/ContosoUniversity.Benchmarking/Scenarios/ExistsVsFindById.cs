#pragma warning disable CA1001
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
public class ExistsVsFindById
{
    private ReadOnlyRepository _roRepository;
    private ReadOnlyContext _roContext;
    private ReadWriteContext _rwContext;
    private Guid[] _coursesExternalIds;

    [GlobalSetup]
    public async Task Setup()
    {
        (_roRepository, _roContext) = RepositoryFactory.CoursesReadOnly();
        (_, _rwContext) = RepositoryFactory.CoursesReadWrite();

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
    public async Task<bool> ExistsById()
    {
        var index = Random.Shared.Next(_coursesExternalIds.Length);
        var id = _coursesExternalIds[index];
        return await _roRepository.Exists(id);
    }

    [Benchmark]
    public async Task<bool> GetById()
    {
        var index = Random.Shared.Next(_coursesExternalIds.Length);
        var id = _coursesExternalIds[index];
        return await _roRepository.GetById(id) == null;
    }
}
