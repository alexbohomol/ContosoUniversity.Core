namespace ContosoUniversity.Benchmarking;

// using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args/*, new DebugInProcessConfig()*/);
}
