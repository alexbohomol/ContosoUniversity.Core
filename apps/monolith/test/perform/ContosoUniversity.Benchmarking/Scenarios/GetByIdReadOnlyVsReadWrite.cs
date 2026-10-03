namespace ContosoUniversity.Benchmarking.Scenarios;

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
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        (_roRepository, _roContext) = RepositoryFactory.CoursesReadOnly();
        (_rwRepository, _rwContext) = RepositoryFactory.CoursesReadWrite();
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
        var id = IdsFactory.SelectCourseIdForIteration(_iteration);
        return await _roRepository.GetById(id) == null;
    }

    [Benchmark]
    public async Task<bool> GetByIdReadWrite()
    {
        _iteration++;
        var id = IdsFactory.SelectCourseIdForIteration(_iteration);
        return await _rwRepository.GetById(id) == null;
    }
}
