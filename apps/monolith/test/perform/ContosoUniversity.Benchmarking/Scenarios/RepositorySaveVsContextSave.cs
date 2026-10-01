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
    private Guid[] _departments;
    private int _maxcredits;

    [GlobalSetup]
    public void Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadWrite();
        _departments =
        [
            new("31a130fe-b396-4bb8-88d3-26fa8778b4c6"),
            new("dab7e678-e3e7-4471-8282-96fe52e5c16f"),
            new("72c0804d-b208-4e67-82ba-cf54dc93dcc8"),
            new("377c186a-6782-4367-9246-e5fe4195a97c")
        ];
        _maxcredits = Credits.MaxValue + 1;
        _iteration = 0;
    }

    [GlobalCleanup]
    public void Cleanup() => _context.Dispose();

    [Benchmark(Baseline = true)]
    public async Task SaveRepository()
    {
        _iteration++;
        var course = GenerateCourse();
        await _repository.Save(course);
    }

    [Benchmark]
    public async Task SaveContext()
    {
        _iteration++;
        var course = GenerateCourse();
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    private Course GenerateCourse()
        => Course.Create(
            1234,
            $"Some course, iteration: {_iteration}",
            _iteration % _maxcredits,
            _departments[_iteration % 4]);
}
