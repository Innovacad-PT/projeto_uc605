namespace store_api.Exceptions;

public class SameIdException : Exception
{

    public SameIdException()
    {
    }

    public SameIdException(string message) : base(message)
    {
    }

    public SameIdException(string message, Exception inner) : base(message, inner)
    {
    }
    
}