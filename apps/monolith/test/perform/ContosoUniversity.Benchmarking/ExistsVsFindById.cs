namespace ContosoUniversity.Benchmarking;

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Data.Courses.Reads;

using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
#pragma warning disable CA1001
public class ExistsVsFindById
#pragma warning restore CA1001
{
    private ReadOnlyRepository _repository;
    private Guid _id;
    private ReadOnlyContext _context;

    [GlobalSetup]
    public void Setup()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
        optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1477;Initial Catalog=ContosoUniversity;User ID=courses_ro;Password=coursesRO-P@$$w0rd;Multiple Active Result Sets=True;Trust Server Certificate=True");
        // optionsBuilder.UseSqlServer("User ID=courses_ro;Password=coursesRO-P@$$w0rd;Server=localhost,1477;Database=ContosoUniversity;MultipleActiveResultSets=True;TrustServerCertificate=True");
        _context = new ReadOnlyContext(optionsBuilder.Options);
        _repository = new ReadOnlyRepository(_context);
        _id = Guid.Parse("f3e9966c-467b-4b99-90ca-a29bae85ca94");
    }

    [GlobalCleanup]
    public void Cleanup() => _context.Dispose();

    [Benchmark(Baseline = true)]
    public async Task<bool> ExistsById() => await _repository.Exists(_id);

    [Benchmark]
    public async Task<bool> GetById() => await _repository.GetById(_id) == null;
}
