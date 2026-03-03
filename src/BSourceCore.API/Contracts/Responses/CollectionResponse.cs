namespace BSourceCore.API.Contracts.Responses;

public record CollectionResponse<T>
{
    public IEnumerable<T> Results { get; set; } = new List<T>();
    public int Total { get; set; }

    public CollectionResponse()
    {        
    }

    public CollectionResponse(IEnumerable<T> results, int total)
    {
        Results = results;
        Total = total;
    }

    public static CollectionResponse<T> From(IEnumerable<T> results)
    {
        return new CollectionResponse<T>(results, results.Count());
    }

    public static CollectionResponse<T> From(IEnumerable<T> results, int total)
    {
        return new CollectionResponse<T>(results, total);
    }

    public static CollectionResponse<T> From(T result)
    {
        return new CollectionResponse<T>(new List<T> { result }, 1);
    }
}
