namespace BSourceCore.Shared.Kernel.Results;
public sealed class CollectionResult<T>
{
    public List<T> Results { get; set; } = new();
    public int Total { get; set; }

    public CollectionResult()
    {        
    }

    public CollectionResult(List<T> results, int total)
    {
        Results = results;
        Total = total;
    }

    public static CollectionResult<T> From(List<T> results)
    {
        return new CollectionResult<T>(results, results.Count);
    }

    public static CollectionResult<T> From(List<T> results, int total)
    {
        return new CollectionResult<T>(results, total);
    }

    public static CollectionResult<T> From(T result)
    {
        return new CollectionResult<T>(new List<T> { result }, 1);
    }

    public static CollectionResult<T> Empty => new CollectionResult<T>(new List<T>(), 0);
}