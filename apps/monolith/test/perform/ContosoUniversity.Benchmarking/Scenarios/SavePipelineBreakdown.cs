namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Writes;

using Domain.Course;

using Factories;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class SavePipelineBreakdown
{
    private ReadWriteContext _context = null!;
    private ReadWriteRepository _repository = null!;

    private Course _course = null!;
    private int _iteration;

    [GlobalSetup]
    public async Task Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();

        const int batchCount = 10;
        const int batchSize = 10_000;

        await _context.PopulateAsync(
            batchCount,
            batchSize,
            CoursesFactory.CreateCourse,
            i => { Console.WriteLine($"Batch #{i}/{batchCount} inserted. {i * batchSize} records. {DateTime.Now:T}"); });
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _context.CleanupAsync<Course>(x => x.Code == 1234);
        await _context.DisposeAsync();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _iteration++;
        _course = CoursesFactory.CreateCourse(_iteration);
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task FindMissingEntity()
    {
        await _context.FindAsync<Course>(_course.ExternalId);
    }

    [Benchmark]
    public async Task AddAsyncOnly()
    {
        await _context.Set<Course>().AddAsync(_course);
    }

    [Benchmark]
    public async Task AddAndSave()
    {
        await _context.Set<Course>().AddAsync(_course);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task FindAddAndSave()
    {
        var existing = await _context.FindAsync<Course>(_course.ExternalId);

        if (existing is null)
        {
            await _context.Set<Course>().AddAsync(_course);
        }

        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task RepositorySave()
    {
        await _repository.Save(_course);
    }
}
