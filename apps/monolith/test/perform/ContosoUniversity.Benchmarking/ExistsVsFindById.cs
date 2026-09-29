#pragma warning disable CA1001
#pragma warning disable CA5394
namespace ContosoUniversity.Benchmarking;

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Reads;

using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
public class ExistsVsFindById
{
    private ReadOnlyRepository _repository;
    private ReadOnlyContext _context;
    private Guid[] _ids;
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
        optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1477;Initial Catalog=ContosoUniversity;User ID=courses_ro;Password=coursesRO-P@$$w0rd;Multiple Active Result Sets=True;Trust Server Certificate=True");
        _context = new ReadOnlyContext(optionsBuilder.Options);
        _repository = new ReadOnlyRepository(_context);
        _ids =
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
    public void Cleanup() => _context.Dispose();

    [Benchmark(Baseline = true)]
    public async Task<bool> ExistsById()
    {
        _iteration++;
        return await _repository.Exists(_ids[_iteration % 7]);
    }

    [Benchmark]
    public async Task<bool> GetById()
    {
        _iteration++;
        return await _repository.GetById(_ids[_iteration % 7]) == null;
    }
}
