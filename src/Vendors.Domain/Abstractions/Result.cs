namespace Vendors.Domain.Abstractions;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("A successful result cannot contain an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("A failing result must contain an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result Of(Func<Result> operation, Func<Exception, Error> onError)
    {
        try
        {
            return operation();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return Failure(onError(exception));
        }
    }

    public static Result<TValue> Of<TValue>(Func<Result<TValue>> operation, Func<Exception, Error> onError)
    {
        try
        {
            return operation();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return Failure<TValue>(onError(exception));
        }
    }

    public static async Task<Result> Of(Func<Task<Result>> operation, Func<Exception, Error> onError)
    {
        try
        {
            return await operation();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return Failure(onError(exception));
        }
    }

    public static async Task<Result<TValue>> Of<TValue>(Func<Task<Result<TValue>>> operation, Func<Exception, Error> onError)
    {
        try
        {
            return await operation();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return Failure<TValue>(onError(exception));
        }
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}
