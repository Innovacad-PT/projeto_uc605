namespace store_api.Exceptions;

public class SameNameException : Exception
{
    public SameNameException()
    {
        
    }

    public SameNameException(string message) : base(message)
    {
        
    }

    public SameNameException(string message, Exception inner) : base(message, inner)
    {
        
    }
}