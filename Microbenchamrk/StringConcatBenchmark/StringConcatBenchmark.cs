using System.Text;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class StringConcatBenchmark
{
    [Params(100, 1000, 10000)]
    public int Size { get; set; }

    private List<int> _values = null!;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _values = Enumerable.Range(0, Size).ToList();
    }
    
    [Benchmark(Baseline = true)] 
    public string StringConcat()
    {
        string result = "";
        foreach (var value in _values)
        {
            result += value.ToString();
        }
        return result;
    }
    
    [Benchmark]
    public string StringBuilderAppend()
    {
        var sb = new StringBuilder(Size * 2);
        foreach (var value in _values)
        {
            sb.Append(value);
        }
        return sb.ToString();
    }
    
    [Benchmark]
    public string StringJoin()
    {
        return string.Join("", _values);
    }
}