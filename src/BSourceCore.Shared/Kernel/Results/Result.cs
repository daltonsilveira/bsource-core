namespace BSourceCore.Shared.Kernel.Results;

public class Result
{
    private readonly List<Error> _errors = new();

    public bool IsSuccess => _errors.Count == 0;
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors => _errors;

    protected Result() { }

    protected void AddError(Error error) => _errors.Add(error);
    protected void AddErrors(IEnumerable<Error> errors) => _errors.AddRange(errors);

    public static Result Success() => new();

    public static Result Fail(Error error)
    {
        var r = new Result();
        r._errors.Add(error);
        return r;
    }

    public static Result Fail(IEnumerable<Error> errors)
    {
        var r = new Result();
        r._errors.AddRange(errors);
        return r;
    }
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) => Value = value;

    private Result(Error error) => AddError(error);

    private Result(IEnumerable<Error> errors) => AddErrors(errors);

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Fail(Error error) => new(error);

    public static new Result<T> Fail(IEnumerable<Error> errors) => new(errors);
}