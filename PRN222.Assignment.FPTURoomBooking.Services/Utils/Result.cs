namespace PRN222.Assignment.FPTURoomBooking.Services.Utils;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public class Result<T> : Result where T : class
{
    public T? Data { get; }

    private Result(bool isSuccess, string? error, T? data) : base(isSuccess, error)
    {
        Data = data;
    }

    private static Result<T> Success(T data) => new(true, null, data);
    public new static Result<T> Failure(string error) => new(false, error, default);

    public static implicit operator Result<T>(T data) => Success(data);
}