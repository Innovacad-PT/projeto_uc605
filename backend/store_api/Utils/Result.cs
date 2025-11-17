namespace store_api.Utils;

public abstract class Result<T>
{
    public ResultCode Code { get; }
    public virtual bool HasError { get; }

    public Result(ResultCode code)
    {
        Code = code;
    }

    public ResultCode GetCode() => Code;
}

public class Success<T> : Result<T> {

    public override bool HasError => false;
    public string Message { get; set; }
    public T Value { get; set; }

    public Success(ResultCode code, string message, T value) : base(code)
    {
        Message = message;
        Value = value;
    }

    public string GetMessage()
    {
        return Message;
    }

    public T GetValue()
    {
        return Value;
    }

}

public class Failure<T> : Result<T>
{
    public override bool HasError => true;

    public string Message { get; }

    public Failure(ResultCode code, string message) : base(code)
    {
        Message = message;
    }

    public string GetMessage()
    {
        return Message;
    }

}

