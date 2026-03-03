namespace BSourceCore.API.Contracts.Responses;

public record ApiResponse<T>
{
    public IEnumerable<T> Results { get; init; } = [];
    public int Total { get; init; }

    public static ApiResponse<T> From(T item) =>
        new() { Results = [item], Total = 1 };

    public static ApiResponse<T> From(IEnumerable<T> items)
    {
        if (items is ICollection<T> collection)
            return new() { Results = collection, Total = collection.Count };

        var list = items.ToList();
        return new() { Results = list, Total = list.Count };
    }
}
