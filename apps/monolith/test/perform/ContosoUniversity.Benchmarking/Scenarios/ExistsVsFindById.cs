#pragma warning disable CA1001
#pragma warning disable CA5394
namespace ContosoUniversity.Benchmarking.Scenarios;

using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Reads;

using Factories;

[MemoryDiagnoser]
public class ExistsVsFindById
{
    private ReadOnlyRepository _repository;
    private ReadOnlyContext _context;
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        (_repository, _context) = RepositoryFactory.CoursesReadOnly();
        _iteration = 0;
    }

    [GlobalCleanup]
    public void Cleanup() => _context.Dispose();

    [Benchmark(Baseline = true)]
    public async Task<bool> ExistsById()
    {
        _iteration++;
        var id = IdsFactory.SelectCourseIdForIteration(_iteration);
        return await _repository.Exists(id);
    }

    [Benchmark]
    public async Task<bool> GetById()
    {
        _iteration++;
        var id = IdsFactory.SelectCourseIdForIteration(_iteration);
        return await _repository.GetById(id) == null;
    }
}
