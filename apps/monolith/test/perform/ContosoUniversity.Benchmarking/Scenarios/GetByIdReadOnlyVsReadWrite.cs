namespace ContosoUniversity.Benchmarking.Scenarios;

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Reads;
using Data.Courses.Writes;

using Factories;

[MemoryDiagnoser]
public class GetByIdReadOnlyVsReadWrite
{
    private ReadOnlyRepository _roRepository;
    private ReadOnlyContext _roContext;
    private ReadWriteRepository _rwRepository;
    private ReadWriteContext _rwContext;
    private Guid[] _courseIds;
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        (_roRepository, _roContext) = RepositoryFactory.CoursesReadOnly();
        (_rwRepository, _rwContext) = RepositoryFactory.CoursesReadWrite();
        _courseIds =
        [
            new("51f60b7d-fb0c-40eb-a74b-b2d90157afa0"),
            new("7f4a2bf3-8623-4d4b-a555-7e1c18da1d31"),
            new("42153736-0a08-49ef-84a1-7718189945ca"),
            new("f3e9966c-467b-4b99-90ca-a29bae85ca94"),
            new("8ebb5543-371a-4c5b-a72b-09bc9f615e36"),
            new("d53ffc3d-aa4e-41cf-8f0e-435c73889dcf"),
            new("1a95b2f1-7f2c-41b4-befb-b0f9c6d991e4")
        ];
        _iteration = 0;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _roContext.Dispose();
        _rwContext.Dispose();
    }

    [Benchmark(Baseline = true)]
    public async Task<bool> GetByIdReadOnly()
    {
        _iteration++;
        return await _roRepository.GetById(_courseIds[_iteration % 7]) == null;
    }

    [Benchmark]
    public async Task<bool> GetByIdReadWrite()
    {
        _iteration++;
        return await _rwRepository.GetById(_courseIds[_iteration % 7]) == null;
    }
}
